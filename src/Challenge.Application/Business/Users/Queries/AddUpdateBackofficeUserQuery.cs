using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Business.Users.Request;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Response;

namespace Challenge.Application.Business.Users.Queries
{
    public class AddUpdateBackofficeUserQuery : IQuery<List<ValidationError>>
    {
        public AddUpdateBackofficeUserRequest Request { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class AddUpdateBackofficeUserQueryHandler : IQueryHandler<AddUpdateBackofficeUserQuery, List<ValidationError>>
    {
        private readonly IRepository<User> _userRepo;
        private readonly Dispatcher _dispatcher;

        public AddUpdateBackofficeUserQueryHandler(IRepository<User> userRepo, Dispatcher dispatcher)
        {
            _userRepo = userRepo;
            _dispatcher = dispatcher;
        }

        public async Task<List<ValidationError>> Handle(AddUpdateBackofficeUserQuery query)
        {
            var result = new List<ValidationError>();

            var adding = string.IsNullOrWhiteSpace(query.Request.Id);
            if (adding)
            {
                if (await _userRepo.AnyAsync(x => x.Email == query.Request.Email))
                {
                    result.Add(new ValidationError
                    {
                        Key = "Email",
                        Value = "Bu e-posta başka bir kullanıcı tarafından kullanılmaktadır."
                    });
                }

                if (await _userRepo.AnyAsync(x => x.PhoneNumber == query.Request.PhoneNumber))
                {
                    result.Add(new ValidationError
                    {
                        Key = "PhoneNumber",
                        Value = "Bu telefon numarası başka bir kullanıcı tarafından kullanılmaktadır."
                    });
                }

                if (result.Any())
                    return result;

                var user = new User
                {
                    FirstName = query.Request.FirstName,
                    LastName = query.Request.LastName,
                    Email = query.Request.Email,
                    PhoneNumber = query.Request.PhoneNumber,
                    Role = query.Request.Role,
                    Password = "asdasd"
                };

                await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
            }
            else
            {
                if (await _userRepo.AnyAsync(x => x.Id != query.Request.Id && x.PhoneNumber == query.Request.PhoneNumber))
                {
                    result.Add(new ValidationError
                    {
                        Key = "PhoneNumber",
                        Value = "Bu telefon numarası başka bir kullanıcı tarafından kullanılmaktadır."
                    });
                }

                if (await _userRepo.AnyAsync(x => x.Id != query.Request.Id && x.Email == query.Request.Email))
                {
                    result.Add(new ValidationError
                    {
                        Key = "Email",
                        Value = "Bu e-posta adresi başka bir kullanıcı tarafından kullanılmaktadır."
                    });
                }

                if (result.Any())
                    return result;

                var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.Request.Id });

                user.FirstName = query.Request.FirstName;
                user.LastName = query.Request.LastName;
                user.Email = query.Request.Email;
                user.PhoneNumber = query.Request.PhoneNumber;
                user.Role = query.Request.Role;

                await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
            }

            return result;
        }
    }
}