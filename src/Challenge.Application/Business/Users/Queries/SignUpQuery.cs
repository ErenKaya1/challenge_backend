using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using ValidationException = Challenge.Core.Exceptions.ValidationException;
using Challenge.Core.Security.Hash;
using Challenge.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignUpQuery : IQuery<User>
    {
        [CustomRequired]
        public string FirstName { get; set; }

        [CustomRequired]
        public string LastName { get; set; }

        [PhoneNumberField]
        public string PhoneNumber { get; set; }

        [EmailField]
        public string Email { get; set; }

        [CustomRequired]
        public string Password { get; set; }

        [CustomRequired]
        [Compare("Password", ErrorMessage = "Parolalar uyuşmuyor.")]
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
        private readonly IHasher _hasher;

        public SignUpQueryHandler(IRepository<User> userRepo, Dispatcher dispatcher, IHasher hasher)
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
            _hasher = hasher;
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
                SetupCompleted = false,
                Password = _hasher.CreateHash(query.Password),
                BanInfo = new BanInfo
                {
                    IsBanned = false,
                    BanReason = null,
                    BanDate = null
                }
            };

            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

            return user;
        }
    }
}