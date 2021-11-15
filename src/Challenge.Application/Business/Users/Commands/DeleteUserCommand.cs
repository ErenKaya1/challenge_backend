using System.Threading.Tasks;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;
using Challenge.Application.Users.Entities;

namespace Challenge.Application.Users.Commands
{
    public class DeleteUserCommand : ICommand
    {
        public Users.Entities.User User { get; set; }
    }

    [AuditLog]
    internal class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly ICrudService<User> _service;

        public DeleteUserCommandHandler(ICrudService<User> service)
        {
            _service = service;
        }

        public Task Handle(DeleteUserCommand command)
        {
            _service.Delete(command.User);
            return Task.CompletedTask;
        }
    }
}