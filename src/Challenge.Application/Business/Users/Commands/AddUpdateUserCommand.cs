using System.Threading.Tasks;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;
using Challenge.Application.Users.Entities;

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

        public AddUpdateUserCommandHandler(ICrudService<User> service)
        {
            _service = service;
        }

        public Task Handle(AddUpdateUserCommand command)
        {
            _service.AddOrUpdate(command.User);
            return Task.CompletedTask;
        }
    }
}