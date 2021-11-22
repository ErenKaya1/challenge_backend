using System.Threading.Tasks;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;
using Challenge.Application.Business.Policies.Entities;

namespace Challenge.Application.Business.Policies.Commands
{
    public class DeletePolicyCommand : ICommand
    {
        public Policy Policy { get; set; }
    }

    [AuditLog]
    internal class DeletePolicyCommandHandler : ICommandHandler<DeletePolicyCommand>
    {
        private readonly ICrudService<Policy> _service;

        public DeletePolicyCommandHandler(ICrudService<Policy> service)
        {
            _service = service;
        }

        public Task Handle(DeletePolicyCommand command)
        {
            _service.Delete(command.Policy);
            return Task.CompletedTask;
        }
    }
}