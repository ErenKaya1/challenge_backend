using System;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Business.Users.EventHandlers
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