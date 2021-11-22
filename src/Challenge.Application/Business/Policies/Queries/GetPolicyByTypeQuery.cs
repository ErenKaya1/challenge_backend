using System.Threading.Tasks;
using Challenge.Application.Business.Policies.Entities;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Policies.Queries
{
    public class GetPolicyByTypeQuery : IQuery<Policy>
    {
        public PolicyType PolicyType { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetPolicyByTypeQueryHandler : IQueryHandler<GetPolicyByTypeQuery, Policy>
    {
        private readonly IRepository<Policy> _policyRepo;

        public GetPolicyByTypeQueryHandler(IRepository<Policy> policyRepo)
        {
            _policyRepo = policyRepo;
        }

        public async Task<Policy> Handle(GetPolicyByTypeQuery query) => await _policyRepo.FirstOrDefaultByAsync(x => x.PolicyType == query.PolicyType);
    }
}