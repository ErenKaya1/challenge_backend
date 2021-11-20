using System;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Commands;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Queries
{
    public class VerifyPhoneNumberAdminQuery : IQuery<VerifyPhoneNumberAdminQueryResult>
    {
        public bool IsPersistent { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class VerifyPhoneNumberAdminQueryHandler : IQueryHandler<VerifyPhoneNumberAdminQuery, VerifyPhoneNumberAdminQueryResult>
    {
        private readonly Dispatcher _dispatcher;
        private readonly IRepository<User> _userRepo;

        public VerifyPhoneNumberAdminQueryHandler(Dispatcher dispatcher, IRepository<User> userRepo)
        {
            _dispatcher = dispatcher;
            _userRepo = userRepo;
        }

        public async Task<VerifyPhoneNumberAdminQueryResult> Handle(VerifyPhoneNumberAdminQuery query)
        {
            var result = new VerifyPhoneNumberAdminQueryResult { HasError = false };

            var user = await _userRepo.FirstOrDefaultByAsync(x => x.Email == query.Email && (x.Role == UserRole.Admin || x.Role == UserRole.Moderator) && x.PhoneNumberConfirmation.Code == query.Code);
            if (user == null)
            {
                result.SetErrorMessage("Hatalı doğrulama kodu.");
                return result;
            }

            if (user.PhoneNumberConfirmation.IsVerified)
            {
                result.SetErrorMessage("Bu doğrulama kodu daha önce kullanılmış. Lütfen yeni bir doğrulama kodu ile yeniden deneyin.");
                return result;
            }

            if (user.PhoneNumberConfirmation.ExpireDate.Value.AddSeconds(120) < DateTime.UtcNow)
            {
                result.SetErrorMessage("Girdiğiniz doğrulama kodunun süresi dolmuş. Lütfen yeni bir doğrulama kodu ile yeniden deneyin.");
                return result;
            }

            user.PhoneNumberConfirmation.IsVerified = true;
            await _dispatcher.Dispatch(new AddUpdateUserCommand { User = user });
            result.User = user;

            return result;
        }
    }

    public class VerifyPhoneNumberAdminQueryResult
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public User User { get; set; }

        public void SetErrorMessage(string message)
        {
            this.HasError = true;
            this.Message = message;
        }
    }
}