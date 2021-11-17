using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;

namespace Challenge.Application.Business.Users.Queries
{
    public class SignInWithGoogleQuery : IQuery<User>
    {

    }

    [AuditLog]
    [DatabaseRetry]
    internal class SignInWithGoogleQueryHandler : IQueryHandler<SignInWithGoogleQuery, User>
    {
        public Task<User> Handle(SignInWithGoogleQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}