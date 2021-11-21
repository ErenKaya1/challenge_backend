using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Attributes;
using Challenge.Core.Exceptions;
using Challenge.Core.Extensions;
using Challenge.Core.Settings;
using FacebookCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInWithFacebookQuery : IQuery<User>
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
    internal class SignInWithFacebookQueryHandler : IQueryHandler<SignInWithFacebookQuery, User>
    {
        private readonly Dispatcher _dispatcher;
        private readonly IRepository<User> _userRepo;
        private readonly AmazonS3Settings _amazonS3Settings;
        private readonly FacebookClientSettings _facebookClientSettings;
        private readonly ILogger<SignInWithFacebookQuery> _logger;

        public SignInWithFacebookQueryHandler(Dispatcher dispatcher,
                                              IRepository<User> userRepo,
                                              IOptions<AmazonS3Settings> amazonS3Settings,
                                              IOptions<FacebookClientSettings> facebookClientSettings,
                                              ILogger<SignInWithFacebookQuery> logger
                                              )
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
            _amazonS3Settings = amazonS3Settings.Value;
            _facebookClientSettings = facebookClientSettings.Value;
            _logger = logger;
        }

        public async Task<User> Handle(SignInWithFacebookQuery query)
        {
            var client = new FacebookClient(_facebookClientSettings.ClientId, _facebookClientSettings.ClientSecret);
            var response = await client.GetAsync("/me?fields=id,first_name,last_name,picture,email", query.AccessToken);

            var id = response.SelectToken("id").ToString();
            var firstName = response.SelectToken("first_name").ToString();
            var lastName = response.SelectToken("last_name").ToString();
            var email = response.SelectToken("email").ToString();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Facebook hesabınızdan bilgilerinizi alamadık, Facebook hesabınızda kayıtlı bir e-posta adresi olmayabilir. Eğer bu hatayı almaya devam ederseniz lütfen diğer oturum açma seçeneklerini kullanın.");

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.UserId });
                if (user == null)
                    throw new UnauthorizedAccessException();

                if (user.ExternalLogins.Any(x => x.ProviderName == "FACEBOOK" && x.ProviderId == id))
                    throw new ValidationException("Facebook hesabınız profilinize zaten bağlı.");

                if (await _userRepo.AnyAsync(x => x.Id != user.Id && x.ExternalLogins.Any(y => y.ProviderId == id && y.ProviderName == "FACEBOOK")))
                    throw new ValidationException("Bu Facebook hesabı başka bir kullanıcı tarafından kullanılıyor.");

                user.ExternalLogins.Add(new ExternalLogin
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    ProviderId = id,
                    ProviderName = "FACEBOOK"
                });

                await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

                return user;
            }
            else
            {
                var user = await _userRepo.FirstOrDefaultByAsync(x => x.ExternalLogins.Any(y => y.ProviderId == id && y.ProviderName == "FACEBOOK"));
                if (user == null)
                {
                    user = await _userRepo.FirstOrDefaultByAsync(x => x.Email == email);
                    if (user == null)
                    {
                        string image = null;
                        try
                        {
                            using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                            {
                                var imageResponse = await httpClient.GetAsync($"https://graph.facebook.com/{id}/picture?type=large");
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
                                    image = _amazonS3Settings.BaseUrl + amazonPath;
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
                            Image = image,
                            SignUpIpAddress = query.IpAddress,
                            ExternalLogins = new List<ExternalLogin>
                            {
                                new ExternalLogin
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    ProviderName = "FACEBOOK",
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
                            ProviderName = "FACEBOOK"
                        });

                        await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
                    }
                }

                return user;
            }
        }
    }
}