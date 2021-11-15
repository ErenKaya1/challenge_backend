using Challenge.Common.Events;
using Challenge.Common.Services;
using Challenge.Application.Users.Entities;

namespace Challenge.Application.Users.Services
{
    public class UserService : CrudService<User>, IUserService
    {
        public UserService(IRepository<User> repo, IDomainEvents domainEvents) : base(repo, domainEvents)
        {

        }
    }
}