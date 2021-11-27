using System.Threading.Tasks;
using Challenge.Common.Commands;
using Challenge.Common.Decorators.AuditLog;
using Challenge.Common.Services;
using Challenge.Application.Business.Localizations.Entities;

namespace Challenge.Application.Business.Localizations.Commands
{
    public class AddUpdateLocalizationCommand : ICommand
    {
        public Localization Localization { get; set; }
    }

    [AuditLog]
    internal class AddUpdateLocalizationCommandHandler : ICommandHandler<AddUpdateLocalizationCommand>
    {
        private readonly ICrudService<Localization> _service;

        public AddUpdateLocalizationCommandHandler(ICrudService<Localization> service)
        {
            _service = service;
        }

        public async Task Handle(AddUpdateLocalizationCommand command)
        {
            await _service.AddOrUpdateAsync(command.Localization);
        }
    }
}