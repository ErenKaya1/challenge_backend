using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Request;
using Challenge.Core.Response;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.Queries
{
    public class GetPoliciesQuery : BaseFilter, IQuery<BaseQueryResult<List<Policy>>>
    {

    }


    [AuditLog]
    [DatabaseRetry]
    internal class GetPoliciesQueryHandler : IQueryHandler<GetPoliciesQuery, BaseQueryResult<List<Policy>>>
    {
        private readonly IRepository<Policy> _repo;

        public GetPoliciesQueryHandler(IRepository<Policy> repo)
        {
            _repo = repo;
        }

        public async Task<BaseQueryResult<List<Policy>>> Handle(GetPoliciesQuery query)
        {
            var result = new BaseQueryResult<List<Policy>>();

            var temp = _repo.GetBy(x => true).Where(x => true).AsQueryable();

            result.Items = temp.OrderByDescending(x => x.CreatedDateTime).Skip(query.Skip).Take(query.Take).ToList();
            result.TotalCount = await _repo.CountAsync(x => true); ;
            result.FilteredCount = temp.Count();
            result.PageCount = result.FilteredCount / query.Take;

            if (result.FilteredCount % query.Take > 0)
                result.PageCount += 1;

            if (query.Skip == 0)
                result.CurrentPage = 1;
            else
                result.CurrentPage = (query.Skip / query.Take) + 1;

            return result;
        }
    }
}