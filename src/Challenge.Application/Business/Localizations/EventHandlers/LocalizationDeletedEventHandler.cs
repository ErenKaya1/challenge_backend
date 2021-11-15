using System;
using Challenge.Common.Events;
using Challenge.Application.Business.Localizations.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Business.Localizations.EventHandlers
{
    public class LocalizationDeletedEventHandler : IDomainEventHandler<EntityDeletedEvent<Localization>>
    {
        private readonly IServiceProvider _serviceProvider;

        public LocalizationDeletedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityDeletedEvent<Localization> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}