using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Exceptions;
using Challenge.Core.Extensions;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using MongoDB.Bson;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInWithAppleQuery : IQuery<User>
    {
        [JsonIgnore]
        public string UserId { get; set; }

        public string AccessToken { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignInWithAppleQueryHandler : IQueryHandler<SignInWithAppleQuery, User>
    {
        private readonly Dispatcher _dispatcher;
        private readonly IRepository<User> _userRepo;

        public SignInWithAppleQueryHandler(Dispatcher dispatcher, IRepository<User> userRepo)
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
        }

        public async Task<User> Handle(SignInWithAppleQuery query)
        {
            string projectId = "";
            FirebaseApp firebaseApp = FirebaseApp.GetInstance(projectId);
            if (firebaseApp == null)
            {
                firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("challenge-firebase.json"),
                    ProjectId = projectId
                }, projectId);
            }

            var defaultAuth = FirebaseAuth.GetAuth(firebaseApp);
            var firebaseToken = await defaultAuth.VerifyIdTokenAsync(query.AccessToken);
            UserRecord userRecord = null;
            if (firebaseToken != null)
                userRecord = await defaultAuth.GetUserAsync(firebaseToken.Uid);

            if (userRecord == null || string.IsNullOrWhiteSpace(userRecord.DisplayName) || string.IsNullOrWhiteSpace(userRecord.Email))
                throw new ValidationException("Apple hesabından e-posta bilgisini alamadık, Apple hesabınızda kayıtlı bir e-posta adresi olmayabilir. Eğer bu hatayı almaya devam ederseniz lütfen diğer oturum açma seçeneklerini kullanın.");

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.UserId });

                if (user.ExternalLogins.Any(x => x.ProviderName == "APPLE" && x.ProviderId == userRecord.Uid))
                    throw new ValidationException("Apple hesabınız profilinize zaten bağlı.");

                if (await _userRepo.AnyAsync(x => x.Id != user.Id && x.ExternalLogins.Any(y => y.ProviderName == "APPLE" && y.ProviderId == userRecord.Uid)))
                    throw new ValidationException("Bu Apple hesabı başka bir kullanıcı tarafından kullanılıyor.");

                user.ExternalLogins.Add(new ExternalLogin
                {
                    ProviderName = "APPLE",
                    ProviderId = userRecord.Uid
                });

                await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

                return user;
            }
            else
            {
                var user = await _userRepo.FirstOrDefaultByAsync(x => x.ExternalLogins.Any(y => y.ProviderName == "APPLE" && y.ProviderId == userRecord.Uid));
                if (user == null)
                {
                    user = await _userRepo.FirstOrDefaultByAsync(x => x.Email == userRecord.Email);
                    if (user == null)
                    {
                        user = new User
                        {
                            Email = userRecord.Email,
                            Password = null,
                            SignUpIpAddress = query.IpAddress,
                            ExternalLogins = new List<ExternalLogin>
                            {
                                new ExternalLogin
                                {
                                    Id = ObjectId.GenerateNewId().ToString(),
                                    ProviderName = "APPLE",
                                    ProviderId = userRecord.Uid
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

                        (user.FirstName, user.LastName) = userRecord.DisplayName.GetFirstAndLastNameByFullName();
                        await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
                    }
                    else
                    {
                        user.ExternalLogins.Add(new ExternalLogin
                        {
                            ProviderName = "APPLE",
                            ProviderId = userRecord.Uid
                        });

                        await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
                    }
                }

                return user;
            }
        }
    }
}