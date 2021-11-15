using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Request;
using Challenge.Core.Response;
using Challenge.Application.Business.Localizations.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Challenge.Application.Business.Localizations.Queries
{
    public class GetLocalizationsQuery : BaseFilter, IQuery<BaseQueryResult<List<Localization>>>
    {

    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetLocalizationsQueryHandler : IQueryHandler<GetLocalizationsQuery, BaseQueryResult<List<Localization>>>
    {
        private readonly IRepository<Localization> _repo;

        public GetLocalizationsQueryHandler(IRepository<Localization> repo)
        {
            _repo = repo;
        }

        public async Task<BaseQueryResult<List<Localization>>> Handle(GetLocalizationsQuery query)
        {
            var result = new BaseQueryResult<List<Localization>>();

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