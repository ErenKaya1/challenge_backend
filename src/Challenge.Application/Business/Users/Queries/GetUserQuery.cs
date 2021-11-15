using System.Threading.Tasks;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Application.Users.Entities;

namespace Challenge.Application.Users.Queries
{
    public class GetUserQuery : IQuery<User>
    {
        public string Id { get; set; }
        public bool ThrowNotFoundIfNull { get; set; }
    }

    [AuditLog]
    internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
    {
        private readonly IRepository<User> _repo;

        public GetUserQueryHandler(IRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task<User> Handle(GetUserQuery query)
        {
            var result = await _repo.FirstOrDefaultByAsync(x => x.Id == query.Id);

            if (query.ThrowNotFoundIfNull && result == null)
                throw new System.Exception($"User with Id: \"{query.Id}\" not found.");

            return result;
        }
    }
}