using System;
using Microsoft.Extensions.DependencyInjection;
using Challenge.Common.Events;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.EventHandlers
{
    public class PolicyUpdatedEventHandler : IDomainEventHandler<EntityUpdatedEvent<Policy>>
    {
        private readonly IServiceProvider _serviceProvider;
        public PolicyUpdatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void Handle(EntityUpdatedEvent<Policy> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}