using System;
using Challenge.Common.Events;
using Challenge.Application.Business.Localizations.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Business.Localizations.EventHandlers
{
    public class LocalizationUpdatedEventHandler : IDomainEventHandler<EntityUpdatedEvent<Localization>>
    {
        private readonly IServiceProvider _serviceProvider;
        public LocalizationUpdatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void Handle(EntityUpdatedEvent<Localization> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}