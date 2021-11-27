using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Attributes;
using Challenge.Core.Response;

namespace Challenge.Application.Business.Users.Queries
{
    public class UpdateAccountAdminQuery : IQuery<List<ValidationError>>
    {
        public string UserId { get; set; }

        [CustomRequired]
        public string FirstName { get; set; }

        [CustomRequired]
        public string LastName { get; set; }

        [CustomRequired]
        [EmailField]
        public string Email { get; set; }

        [CustomRequired]
        [PhoneNumberField]
        public string PhoneNumber { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class UpdateAccountAdminQueryHandler : IQueryHandler<UpdateAccountAdminQuery, List<ValidationError>>
    {
        private readonly Dispatcher _dispatcher;
        private readonly IRepository<User> _userRepo;

        public UpdateAccountAdminQueryHandler(Dispatcher dispatcher, IRepository<User> userRepo)
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
        }

        public async Task<List<ValidationError>> Handle(UpdateAccountAdminQuery query)
        {
            var result = new List<ValidationError>();

            if (await _userRepo.AnyAsync(x => x.Id != query.UserId && x.PhoneNumber == query.PhoneNumber))
            {
                result.Add(new ValidationError
                {
                    Key = "PhoneNumber",
                    Value = "Bu telefon numarası başka bir kullanıcı tarafından kullanılıyor."
                });
            }

            if (await _userRepo.AnyAsync(x => x.Id != query.UserId && x.Email == query.Email))
            {
                result.Add(new ValidationError
                {
                    Key = "Email",
                    Value = "Bu e-posta adresi başka bir kullanıcı tarafından kullanılıyor."
                });
            }

            if (result.Any())
                return result;

            var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.UserId });
            if (user == null)
                throw new UnauthorizedAccessException();

            user.FirstName = query.FirstName;
            user.LastName = query.LastName;
            user.Email = query.Email;
            user.PhoneNumber = query.PhoneNumber;

            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

            return result;
        }
    }
}