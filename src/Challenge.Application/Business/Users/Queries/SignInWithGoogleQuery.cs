using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Business.Users.Response;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Attributes;
using Challenge.Core.Exceptions;
using Challenge.Core.Extensions;
using Challenge.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInWithGoogleQuery : IQuery<User>
    {
        [JsonIgnore]
        [MongoIdField]
        public string UserId { get; set; }

        [CustomRequired]
        public string AccessToken { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignInWithGoogleQueryHandler : IQueryHandler<SignInWithGoogleQuery, User>
    {
        private readonly Dispatcher _dispatcher;
        private readonly ILogger<SignInWithGoogleQuery> _logger;
        private readonly IRepository<User> _userRepo;
        private readonly AmazonS3Settings _amazonS3Settings;

        public SignInWithGoogleQueryHandler(Dispatcher dispatcher,
                                            ILogger<SignInWithGoogleQuery> logger,
                                            IRepository<User> userRepo,
                                            IOptions<AmazonS3Settings> amazonS3Settings
                                            )
        {
            _dispatcher = dispatcher;
            _logger = logger;
            _userRepo = userRepo;
            _amazonS3Settings = amazonS3Settings.Value;
        }

        public async Task<User> Handle(SignInWithGoogleQuery query)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUrl = string.Format("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}", query.AccessToken);
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                var httpResponse = await httpClient.SendAsync(httpRequest);
                var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<GoogleTokenValidationResponse>(responseStream);

                var id = result.sub;
                var firstName = result.given_name;
                var lastName = result.family_name;
                var email = result.email;
                var pictureUrl = result.picture;

                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(email))
                    throw new ValidationException("Google hesabınızdan bilgilerinizi alamadık, Google hesabınızda kayıtlı bir e-posta adresi olmayabilir. Eğer bu hatayı almaya devam ederseniz lütfen diğer oturum açma seçeneklerini kullanın.");

                if (!string.IsNullOrWhiteSpace(query.UserId))
                {
                    var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.UserId });

                    if (user.ExternalLogins.Any(x => x.ProviderName == "GOOGLE" && x.ProviderId == id))
                        throw new ValidationException("Google hesabınız profilinize zaten bağlı.");

                    if (await _userRepo.AnyAsync(x => x.Id != query.UserId && x.ExternalLogins.Any(y => y.ProviderId == id && y.ProviderName == "GOOGLE")))
                        throw new ValidationException("Bu Google hesabı başka bir kullanıcı tarafından kullanılıyor.");

                    user.ExternalLogins.Add(new ExternalLogin
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        ProviderId = id,
                        ProviderName = "GOOGLE"
                    });

                    await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

                    return user;
                }
                else
                {
                    var user = await _userRepo.FirstOrDefaultByAsync(x => x.ExternalLogins.Any(y => y.ProviderId == id && y.ProviderName == "GOOGLE"));
                    if (user == null)
                    {
                        user = await _userRepo.FirstOrDefaultByAsync(x => x.Email == email);
                        if (user == null)
                        {
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(pictureUrl) && pictureUrl.Contains("="))
                                {
                                    pictureUrl = pictureUrl.Split("=")[0];
                                    pictureUrl += "=s500";
                                }

                                using (var imageClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                                {
                                    var imageResponse = await imageClient.GetAsync(pictureUrl);
                                    var stream = await imageResponse.Content.ReadAsStreamAsync();
                                    var amazonPath = "user/profile/" + Guid.NewGuid() + ".jpg";

                                    var amazonClient = new AmazonS3Client(_amazonS3Settings.AccessKey, _amazonS3Settings.AccessSecret, RegionEndpoint.EUCentral1);
                                    var request = new PutObjectRequest
                                    {
                                        BucketName = _amazonS3Settings.Bucket,
                                        Key = amazonPath,
                                        InputStream = stream,
                                        ContentType = "image/jpeg",
                                        CannedACL = S3CannedACL.PublicRead
                                    };

                                    var putResponse = await amazonClient.PutObjectAsync(request);
                                    if (putResponse.HttpStatusCode == HttpStatusCode.OK)
                                        pictureUrl = _amazonS3Settings.BaseUrl + amazonPath;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message);
                            }

                            user = new User
                            {
                                Email = email,
                                Password = null,
                                Image = pictureUrl,
                                SignUpIpAddress = query.IpAddress,
                                ExternalLogins = new List<ExternalLogin>
                            {
                                new ExternalLogin
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    ProviderName = "GOOGLE",
                                    ProviderId = id
                                }
                            },
                                EmailConfirmation = new EmailConfirmation
                                {
                                    IsVerified = true
                                },
                                PhoneNumberConfirmation = new PhoneNumberConfirmation
                                {
                                    Code = null,
                                    IsVerified = false,
                                    LastCodeSendDate = null
                                },
                                BanInfo = new BanInfo
                                {
                                    IsBanned = false,
                                    BanReason = null,
                                    BanDate = null
                                },
                                Role = UserRole.User,
                                SetupCompleted = false,
                                HideMyData = false,
                            };

                            (user.FirstName, user.LastName) = (firstName + " " + lastName).GetFirstAndLastNameByFullName();
                            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
                        }
                        else
                        {
                            user.ExternalLogins.Add(new ExternalLogin
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                ProviderId = id,
                                ProviderName = "GOOGLE"
                            });

                            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
                        }
                    }

                    return user;
                }
            }
        }
    }
}