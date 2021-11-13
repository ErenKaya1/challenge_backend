using System.Threading.Tasks;
using Challenge.Common.Commands;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Challenge.Common.Decorators.AuditLog
{
    [Mapping(Type = typeof(AuditLogAttribute))]
    public class AuditLogCommandDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly ILogger<AuditLogCommandDecorator<TCommand>> logger;

        public AuditLogCommandDecorator(
               ICommandHandler<TCommand> handler,
            Microsoft.Extensions.Logging.ILogger<AuditLogCommandDecorator<TCommand>> logger
            )
        {
            _handler = handler;
            this.logger = logger;
        }

        public async Task Handle(TCommand command)
        {
            var commandJson = JsonConvert.SerializeObject(command);
            // Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");
            ColorConsole.WriteEmbeddedColorLine(
                $"[green]Command of type[/green] [blue]{command.GetType().Name}[/blue] : {commandJson}");

            logger.LogInformation("Query Executed: {@Data}", new
            {
                CommandName = command.GetType().Name,
                CommandJson = commandJson
            });

            await _handler.Handle(command);
        }
    }
}