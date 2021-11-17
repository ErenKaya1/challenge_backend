using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Events;
using Challenge.Common.Services;

namespace Challenge.Application.Business.Users.Services
{
    public class UserService : CrudService<User>, IUserService
    {
        public UserService(IRepository<User> repo, IDomainEvents domainEvents) : base(repo, domainEvents)
        {

        }
    }
}