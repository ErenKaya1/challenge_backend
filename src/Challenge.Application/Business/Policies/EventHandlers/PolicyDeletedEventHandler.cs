using System;
using Microsoft.Extensions.DependencyInjection;
using Challenge.Common.Events;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.EventHandlers
{
    public class PolicyDeletedEventHandler : IDomainEventHandler<EntityDeletedEvent<Policy>>
    {
        private readonly IServiceProvider _serviceProvider;

        public PolicyDeletedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityDeletedEvent<Policy> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}