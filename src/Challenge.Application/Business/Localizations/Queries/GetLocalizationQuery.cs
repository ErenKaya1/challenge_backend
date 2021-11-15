using System.Threading.Tasks;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Application.Business.Localizations.Entities;

namespace Challenge.Application.Business.Localizations.Queries
{
    public class GetLocalizationQuery : IQuery<Localization>
    {
        public string Id { get; set; }
        public bool ThrowNotFoundIfNull { get; set; }
    }

    [AuditLog]
    internal class GetLocalizationQueryHandler : IQueryHandler<GetLocalizationQuery, Localization>
    {
        private readonly IRepository<Localization> _repo;

        public GetLocalizationQueryHandler(IRepository<Localization> repo)
        {
            _repo = repo;
        }

        public async Task<Localization> Handle(GetLocalizationQuery query)
        {
            var result = await _repo.FirstOrDefaultByAsync(x => x.Id == query.Id);

            if (query.ThrowNotFoundIfNull && result == null)
            {
                throw new System.Exception($"Localization with Id: \"{query.Id}\" not found.");
            }

            return result;
        }
    }
}