using System.Threading.Tasks;
using Challenge.Common.Queries;

namespace Challenge.Common.Decorators.DatabaseRetry
{
    [Mapping(Type = typeof(DatabaseRetryAttribute))]
    public class DatabaseRetryQueryDecorator<TQuery, TResult> : DatabaseRetryDecoratorBase, IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;

        public DatabaseRetryQueryDecorator(IQueryHandler<TQuery, TResult> handler, DatabaseRetryAttribute options)
        {
            DatabaseRetryOptions = options;
            _handler = handler;
        }

        public Task<TResult> Handle(TQuery query)
        {
            Task<TResult> result = default;
            WrapExecution(() => result = _handler.Handle(query));
            return result;
        }
    }
}