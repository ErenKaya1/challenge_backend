using System.Threading.Tasks;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Decorators.DatabaseRetry;
using Challenge.Common.Queries;
using Challenge.Common.Services;
using Challenge.Application.Business.Localizations.Entities;

namespace Challenge.Application.Business.Localizations.Queries
{
    public class GetLocalizationByKeyAndLanguageCodeQuery : IQuery<Localization>
    {
        public string Key { get; set; }
        public string LanguageCode { get; set; }
    }

    [AuditLog]
    [DatabaseRetry]
    internal class GetLocalizationByKeyAndLanguageCodeQueryHandler : IQueryHandler<GetLocalizationByKeyAndLanguageCodeQuery, Localization>
    {
        private readonly IRepository<Localization> _localizationRepo;

        public GetLocalizationByKeyAndLanguageCodeQueryHandler(IRepository<Localization> localizationRepo)
        {
            _localizationRepo = localizationRepo;
        }

        public async Task<Localization> Handle(GetLocalizationByKeyAndLanguageCodeQuery query) => await _localizationRepo.FirstOrDefaultByAsync(x => x.Key == query.Key && x.LanguageCode == query.LanguageCode);
    }
}