using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Core.Attributes;
using Challenge.Core.Response;
using Challenge.Core.Security.Hash;

namespace Challenge.Application.Business.Users.Queries
{
    public class UpdatePasswordAdminQuery : IQuery<List<ValidationError>>
    {
        public string UserId { get; set; }

        [CustomRequired]
        public string CurrentPassword { get; set; }

        [CustomRequired]
        public string NewPassword { get; set; }

        [CustomRequired]
        [Compare("NewPassword", ErrorMessage = "Parolalar uyuşmuyor.")]
        public string NewPasswordConfirm { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class UpdatePasswordAdminQueryHandler : IQueryHandler<UpdatePasswordAdminQuery, List<ValidationError>>
    {
        private readonly Dispatcher _dispatcher;
        private readonly IHasher _hasher;

        public UpdatePasswordAdminQueryHandler(Dispatcher dispatcher, IHasher hasher)
        {
            _dispatcher = dispatcher;
            _hasher = hasher;
        }

        public async Task<List<ValidationError>> Handle(UpdatePasswordAdminQuery query)
        {
            var result = new List<ValidationError>();
            var user = await _dispatcher.Dispatch(new GetUserQuery { Id = query.UserId });
            if (user == null)
                throw new UnauthorizedAccessException();

            if (user.Password != _hasher.CreateHash(query.CurrentPassword))
            {
                result.Add(new ValidationError
                {
                    Key = "CurrentPassword",
                    Value = "Eski parola hatalı"
                });
            }

            if (result.Any())
                return result;

            user.Password = _hasher.CreateHash(query.NewPassword);
            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });

            return result;
        }
    }
}