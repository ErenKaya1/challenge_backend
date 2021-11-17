using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Services.Cache;
using Challenge.Application.Services.Cache.Redis;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Core.Exceptions;

namespace Challenge.Application.Business.Users.Queries
{
    public class GetUserQuery : IQuery<User>
    {
        public string Id { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
    {
        private readonly IRepository<User> _repo;
        private readonly IRedisService _redisService;

        public GetUserQueryHandler(IRepository<User> repo, IRedisService redisService)
        {
            _repo = repo;
            _redisService = redisService;
        }

        public async Task<User> Handle(GetUserQuery query)
        {
            var user = await _redisService.GetAsync<User>($"user-{query.Id}", CacheTimes.CACHE_120_DK, async () =>
            {
                return await _repo.FirstOrDefaultByAsync(x => x.Id == query.Id);
            });

            if (user == null)
                throw new NotFoundException("Kullanıcı bulunamadı.");

            return user;
        }
    }
}