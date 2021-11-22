using System.Threading.Tasks;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;
using Challenge.Application.Business.Policies.Entities;
using Challenge.Core.Extensions;

namespace Challenge.Application.Business.Policies.Commands
{
    public class AddUpdatePolicyCommand : ICommand
    {
        public Policy Policy { get; set; }
    }

    [AuditLog]
    internal class AddUpdatePolicyCommandHandler : ICommandHandler<AddUpdatePolicyCommand>
    {
        private readonly ICrudService<Policy> _service;

        public AddUpdatePolicyCommandHandler(ICrudService<Policy> service)
        {
            _service = service;
        }

        public Task Handle(AddUpdatePolicyCommand command)
        {
            command.Policy.Slug = command.Policy.Title.ToUrlSlug();
            _service.AddOrUpdate(command.Policy);
            
            return Task.CompletedTask;
        }
    }
}