using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Attributes;
using Challenge.Core.Exceptions;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInWithEmailQuery : IQuery<User>
    {
        [CustomRequired]
        [EmailField]
        public string Email { get; set; }

        [CustomRequired]
        public string Password { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignInWithPhoneNumberQueryHandler : IQueryHandler<SignInWithEmailQuery, User>
    {
        private readonly IRepository<User> _userRepo;

        public SignInWithPhoneNumberQueryHandler(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User> Handle(SignInWithEmailQuery query)
        {
            var user = await _userRepo.FirstOrDefaultByAsync(x => x.Email == query.Email && x.Password == query.Password);
            if (user == null)
                throw new ValidationException("Hatalı kullanıcı adı veya parola girdiniz.");

            return user;
        }
    }
}