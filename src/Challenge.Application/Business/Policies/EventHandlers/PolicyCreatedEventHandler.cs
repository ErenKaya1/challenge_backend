using System;
using Microsoft.Extensions.DependencyInjection;
using Challenge.Common.Events;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.EventHandlers
{
    public class PolicyCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<Policy>>
    {
        private readonly IServiceProvider _serviceProvider;

        public PolicyCreatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityCreatedEvent<Policy> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}