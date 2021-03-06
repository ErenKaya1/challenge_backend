using System.Threading.Tasks;
using Challenge.Common.Commands;

namespace Challenge.Common.Decorators.DatabaseRetry
{
    [Mapping(Type = typeof(DatabaseRetryAttribute))]
    public class DatabaseRetryCommandDecorator<TCommand> : DatabaseRetryDecoratorBase, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        public DatabaseRetryCommandDecorator(ICommandHandler<TCommand> handler, DatabaseRetryAttribute options)
        {
            DatabaseRetryOptions = options;
            _handler = handler;
        }

        public Task Handle(TCommand command)
        {
            WrapExecution(() => _handler.Handle(command));
            return Task.CompletedTask;
        }
    }
}
