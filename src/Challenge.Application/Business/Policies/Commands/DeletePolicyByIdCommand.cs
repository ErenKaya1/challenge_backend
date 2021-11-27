using System.Threading.Tasks;
using Challenge.Application.Business.Policies.Entities;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;

namespace Challenge.Application.Business.Policies.Commands
{
    public class DeletePolicyByIdCommand : ICommand
    {
        public string Id { get; set; }
    }

    [AuditLog]
    internal class DeletePolicyByIdCommandHandler : ICommandHandler<DeletePolicyByIdCommand>
    {
        private readonly ICrudService<Policy> _service;

        public DeletePolicyByIdCommandHandler(ICrudService<Policy> service)
        {
            _service = service;
        }

        public async Task Handle(DeletePolicyByIdCommand command)
        {
            await _service.DeleteAsync(command.Id);
        }
    }
}