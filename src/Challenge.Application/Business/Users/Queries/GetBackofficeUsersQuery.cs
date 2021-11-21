using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Request;
using Challenge.Core.Response;
using NinjaNye.SearchExtensions;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Application.Business.Users.Queries
{
    public class GetBackofficeUsersQuery : BaseFilter, IQuery<BaseQueryResult<List<User>>>
    {
        public UserRole? Role { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetBackofficeUsersQueryHandler : IQueryHandler<GetBackofficeUsersQuery, BaseQueryResult<List<User>>>
    {
        private readonly IRepository<User> _userRepo;

        public GetBackofficeUsersQueryHandler(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<BaseQueryResult<List<User>>> Handle(GetBackofficeUsersQuery query)
        {
            var result = new BaseQueryResult<List<User>>();

            var temp = _userRepo.GetBy(x => x.Role == UserRole.Admin || x.Role == UserRole.Moderator).AsQueryable();

            if (query.Role.HasValue)
                temp = temp.Where(x => x.Role == query.Role.Value).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                temp = temp.Search(x => x.FirstName.ToLower(),
                                   x => x.LastName.ToLower(),
                                   x => x.Email.ToLower(),
                                   x => x.PhoneNumber.ToLower()).Containing(query.SearchTerm.ToLower()).AsQueryable();
            }

            result.Items = temp.OrderByDescending(x => x.CreatedDateTime).Skip(query.Skip).Take(query.Take).ToList();
            result.TotalCount = await _userRepo.CountAsync(x => true); ;
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