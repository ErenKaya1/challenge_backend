using System;
using Challenge.Common.Events;
using Challenge.Application.Users.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Users.EventHandlers
{
    public class UserUpdatedEventHandler : IDomainEventHandler<EntityUpdatedEvent<User>>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserUpdatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityUpdatedEvent<User> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
               
            }
        }
    }
}