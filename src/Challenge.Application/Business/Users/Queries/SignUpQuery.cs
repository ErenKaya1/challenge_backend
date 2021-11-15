using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Users.Entities;
using Challenge.Core.Exceptions;
using Challenge.Application.Services.Localization;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignUpQuery : IQuery<User>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Image { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignUpQueryHandler : IQueryHandler<SignUpQuery, User>
    {
        private readonly IRepository<User> _userRepo;
        private readonly Dispatcher _dispatcher;
        private readonly ILocalizationService _localizer;

        public SignUpQueryHandler(IRepository<User> userRepo, Dispatcher dispatcher, ILocalizationService localizer)
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
            _localizer = localizer;
        }

        public async Task<User> Handle(SignUpQuery query)
        {
            if (await _userRepo.AnyAsync(x => x.Email == query.Email))
                throw new ValidationException(await _localizer.ParseAsync("duplicate-email-error", "Bu e-posta adresi başka bir kullanıcı tarafından kullanılıyor."));

            if (await _userRepo.AnyAsync(x => x.Username == query.Username))
                throw new ValidationException(await _localizer.ParseAsync("duplicate-username-error", "Bu kullanıcı adı başka bir kullanıcı tarafından kullanılıyor."));

            var user = new User
            {
                Username = query.Username,
                Email = query.Email,
                FirstName = query.FirstName,
                LastName = query.LastName,
                IsBanned = false,
            };

            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

            return user;
        }
    }
}