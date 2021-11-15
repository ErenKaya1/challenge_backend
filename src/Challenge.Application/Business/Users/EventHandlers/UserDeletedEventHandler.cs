using System;
using Challenge.Common.Events;
using Challenge.Application.Users.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Users.EventHandlers
{
    public class UserDeletedEventHandler : IDomainEventHandler<EntityDeletedEvent<User>>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserDeletedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(EntityDeletedEvent<User> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
            }
        }
    }
}