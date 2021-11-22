using System.Threading.Tasks;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Core.Exceptions;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.Queries
{
    public class GetPolicyQuery : IQuery<Policy>
    {
        public string Id { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetPolicyQueryHandler : IQueryHandler<GetPolicyQuery, Policy>
    {
        private readonly IRepository<Policy> _repo;

        public GetPolicyQueryHandler(IRepository<Policy> repo)
        {
            _repo = repo;
        }

        public async Task<Policy> Handle(GetPolicyQuery query)
        {
            var result = await _repo.FirstOrDefaultByAsync(x => x.Id == query.Id);

            if (result == null)
                throw new NotFoundException($"Policy {query.Id} not found.");

            return result;
        }
    }
}