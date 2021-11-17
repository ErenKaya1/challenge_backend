using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Application.Services.Cache;
using Challenge.Application.Services.Cache.Redis;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;

namespace Challenge.Application.Business.Users.Commands
{
    public class AddUpdateUserCommand : ICommand
    {
        public User User { get; set; }
    }

    [AuditLog]
    internal class AddUpdateUserCommandHandler : ICommandHandler<AddUpdateUserCommand>
    {
        private readonly ICrudService<User> _service;
        private readonly IRedisService _redisService;

        public AddUpdateUserCommandHandler(ICrudService<User> service, IRedisService redisService)
        {
            _service = service;
            _redisService = redisService;
        }

        public async Task Handle(AddUpdateUserCommand command)
        {
            _service.AddOrUpdate(command.User);
            await _redisService.AddAsync($"user-{command.User.Id}", command.User, CacheTimes.CACHE_120_DK);
        }
    }
}