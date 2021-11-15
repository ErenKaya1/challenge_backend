using System;
using Challenge.Common.Events;
using Challenge.Application.Business.Localizations.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Business.Localizations.EventHandlers
{
    public class LocalizationCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Localization>>
    {
        private readonly IServiceProvider _serviceProvider;

        public LocalizationCreatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityCreatedEvent<Localization> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}