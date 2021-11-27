using System;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Attributes;
using Challenge.Core.Security.Hash;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInAdminQuery : IQuery<User>
    {
        [CustomRequired]
        [EmailField]
        public string Email { get; set; }

        [CustomRequired]
        public string Password { get; set; }
    }

    [AuditLog]
    internal class SignInAdminQueryHandler : IQueryHandler<SignInAdminQuery, User>
    {
        private readonly IRepository<User> _userRepo;
        private readonly IHasher _hasher;
        private readonly Dispatcher _dispatcher;

        public SignInAdminQueryHandler(IRepository<User> userRepo, IHasher hasher, Dispatcher dispatcher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
            _dispatcher = dispatcher;
        }

        public async Task<User> Handle(SignInAdminQuery query)
        {
            if (!await _userRepo.AnyAsync(x => x.Email == "kayainternative@gmail.com"))
            {
                var defaultAdmin = new User
                {
                    FirstName = "Eren",
                    LastName = "Kaya",
                    Email = "kayainternative@gmail.com",
                    PhoneNumber = "+905396046268",
                    Password = "123456",
                    Role = UserRole.Admin,
                };

                await _dispatcher.Dispatch(new AddUpdateUserCommand { User = defaultAdmin });
            }

            var user = await _userRepo.FirstOrDefaultByAsync(x => x.Email == query.Email && x.Password == _hasher.CreateHash(query.Password));
            if (user != null)
            {
                user.PhoneNumberConfirmation = new PhoneNumberConfirmation
                {
                    Code = "0000",
                    ExpireDate = DateTime.UtcNow.AddSeconds(120),
                    LastCodeSendDate = DateTime.UtcNow,
                    IsVerified = false
                };

                await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
            }

            return user;
        }
    }
}