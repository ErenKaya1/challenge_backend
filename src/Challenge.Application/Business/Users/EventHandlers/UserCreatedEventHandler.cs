using System;
using Challenge.Common.Events;
using Challenge.Application.Users.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Users.EventHandlers
{
    public class UserCreatedEventHandler : IDomainEventHandler<EntityCreatedEvent<User>>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserCreatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityCreatedEvent<User> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}