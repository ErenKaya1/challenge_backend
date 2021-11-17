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
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

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

            if (userRecord == null)
                throw new ValidationException("Apple ID kullanıcı bilgilerini doğrulayamadı.");

            if (string.IsNullOrWhiteSpace(userRecord.Email))
                throw new ValidationException("Apple hesabından e-posta bilgisini alamadık.");

            if (string.IsNullOrWhiteSpace(userRecord.DisplayName))
                throw new ValidationException("Apple hesabından ad-soyad bilgilerini alamadık.");

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.UserId });

                if (user.ExternalLogins.Any(x => x.ProviderName == "APPLE" && x.ProviderId == userRecord.Uid))
                    throw new ValidationException("Apple hesabınız profilinize zaten bağlı.");

                var control = await _userRepo.AnyAsync(x => x.Id != user.Id && x.ExternalLogins.Any(y => y.ProviderName == "APPLE" && y.ProviderId == userRecord.Uid));
                if (control)
                    throw new ValidationException("Bu hesap başka bir kullanıcı tarafından kullanılıyor.");

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
                        var password = "asdasd";
                        user = await _dispatcher.Dispatch(new SignUpQuery
                        {
                            Email = userRecord.Email,
                            FirstName = "",
                            LastName = "",
                            Password = password,
                            PasswordConfirm = password,
                            IpAddress = query.IpAddress
                        });
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