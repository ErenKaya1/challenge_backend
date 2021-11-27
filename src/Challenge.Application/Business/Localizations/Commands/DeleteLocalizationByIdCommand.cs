using System.Threading.Tasks;
using Challenge.Application.Business.Localizations.Entities;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;

namespace Challenge.Application.Business.Localizations.Commands
{
    public class DeleteLocalizationByIdCommand : ICommand
    {
        public string Id { get; set; }
    }

    [AuditLog]
    internal class DeleteLocalizationByIdCommandHandler : ICommandHandler<DeleteLocalizationByIdCommand>
    {
        private readonly ICrudService<Localization> _service;

        public DeleteLocalizationByIdCommandHandler(ICrudService<Localization> service)
        {
            _service = service;
        }

        public async Task Handle(DeleteLocalizationByIdCommand command)
        {
            await _service.DeleteAsync(command.Id);
        }
    }
}