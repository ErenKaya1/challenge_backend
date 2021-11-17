using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using System.ComponentModel.DataAnnotations;
using ValidationException = Challenge.Core.Exceptions.ValidationException;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignUpQuery : IQuery<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignUpQueryHandler : IQueryHandler<SignUpQuery, User>
    {
        private readonly IRepository<User> _userRepo;
        private readonly Dispatcher _dispatcher;

        public SignUpQueryHandler(IRepository<User> userRepo, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
        }

        public async Task<User> Handle(SignUpQuery query)
        {
            if (await _userRepo.AnyAsync(x => x.Email == query.Email))
                throw new ValidationException("Bu e-posta adresi başka bir kullanıcı tarafından kullanılıyor.");

            if (!string.IsNullOrWhiteSpace(query.PhoneNumber) && await _userRepo.AnyAsync(x => x.PhoneNumber == query.PhoneNumber))
                throw new ValidationException("Bu telefon numarası başka bir kullanıcı tarafından kullanılıyor.");

            if (query.Password != query.PasswordConfirm)
                throw new ValidationException("Parolalar uyuşmuyor.");

            var user = new User
            {
                Email = query.Email,
                FirstName = query.FirstName,
                LastName = query.LastName,
                SignUpIpAddress = query.IpAddress,
                IsBanned = false,
                SetupCompleted = false
            };

            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

            return user;
        }
    }
}