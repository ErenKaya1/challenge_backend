using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Request;
using Challenge.Core.Response;
using Challenge.Application.Users.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Challenge.Application.Users.Queries
{
    public class GetUsersQuery : BaseFilter, IQuery<BaseQueryResult<List<User>>>
    {

    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, BaseQueryResult<List<User>>>
    {
        private readonly IRepository<User> _repo;

        public GetUsersQueryHandler(IRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<BaseQueryResult<List<User>>> Handle(GetUsersQuery query)
        {
            var result = new BaseQueryResult<List<User>>();

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