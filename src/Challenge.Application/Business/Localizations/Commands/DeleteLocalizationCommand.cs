using System.Threading.Tasks;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;
using Challenge.Application.Business.Localizations.Entities;

namespace Challenge.Application.Business.Localizations.Commands
{
    public class DeleteLocalizationCommand : ICommand
    {
        public Localization Localization { get; set; }
    }

    [AuditLog]
    internal class DeleteLocalizationCommandHandler : ICommandHandler<DeleteLocalizationCommand>
    {
        private readonly ICrudService<Localization> _service;

        public DeleteLocalizationCommandHandler(ICrudService<Localization> service)
        {
            _service = service;
        }

        public Task Handle(DeleteLocalizationCommand command)
        {
            _service.Delete(command.Localization);
            return Task.CompletedTask;
        }
    }
}