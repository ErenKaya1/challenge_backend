using System;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Business.Users.EventHandlers
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