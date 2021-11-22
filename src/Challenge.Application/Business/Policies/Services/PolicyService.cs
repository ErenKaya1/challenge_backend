using Challenge.Common.Events;
using Challenge.Common.Services;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.Services
{
    public class PolicyService : CrudService<Policy>, IPolicyService
    {
        public PolicyService(IRepository<Policy> repo, IDomainEvents domainEvents) : base(repo, domainEvents)
        {

        }
    }
}