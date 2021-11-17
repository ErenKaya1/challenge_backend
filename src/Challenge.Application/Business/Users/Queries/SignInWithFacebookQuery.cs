using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInWithFacebookQuery : IQuery<User>
    {

    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignInWithFacebookQueryHandler : IQueryHandler<SignInWithFacebookQuery, User>
    {
        public Task<User> Handle(SignInWithFacebookQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}