using Challenge.Common.Events;
using Challenge.Common.Services;
using Challenge.Application.Business.Localizations.Entities;

namespace Challenge.Application.Business.Localizations.Services
{
    public class LocalizationService : CrudService<Localization>, ILocalizationService
    {
        public LocalizationService(IRepository<Localization> repo, IDomainEvents domainEvents) : base(repo, domainEvents)
        {

        }
    }
}