using System;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Services.Cache.Redis;
using Challenge.Common.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.Application.Business.Users.EventHandlers
{
    public class UserDeletedEventHandler : IDomainEventHandler<EntityDeletedEvent<User>>
    {
        private readonly IServiceProvider _serviceProvider;

        public UserDeletedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async void Handle(EntityDeletedEvent<User> domainEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
                await redisService.RemoveAsync($"user-{domainEvent.Entity.Id}");
            }
        }
    }
}