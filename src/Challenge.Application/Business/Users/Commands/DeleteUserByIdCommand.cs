using System.Threading.Tasks;
using Challenge.Application.Business.Users.Entities;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;

namespace Challenge.Application.Business.Users.Commands
{
    public class DeleteUserByIdCommand : ICommand
    {
        public string Id { get; set; }
    }

    [AuditLog]
    internal class DeleteUserByIdCommandHandler : ICommandHandler<DeleteUserByIdCommand>
    {
        private readonly ICrudService<User> _service;

        public DeleteUserByIdCommandHandler(ICrudService<User> service)
        {
            _service = service;
        }

        public async Task Handle(DeleteUserByIdCommand command)
        {
            await _service.DeleteAsync(command.Id);
        }
    }
}