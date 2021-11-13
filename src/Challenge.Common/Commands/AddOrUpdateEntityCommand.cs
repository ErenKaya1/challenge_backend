using System.Threading.Tasks;
using Challenge.Common.Services;

namespace Challenge.Common.Commands
{
    public class AddOrUpdateEntityCommand<TEntity> : ICommand
        where TEntity : AggregateRoot<string>
    {
        public AddOrUpdateEntityCommand(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; set; }
    }

    public class AddOrUpdateEntityCommandHandler<TEntity> : ICommandHandler<AddOrUpdateEntityCommand<TEntity>>
    where TEntity : AggregateRoot<string>
    {
        private readonly ICrudService<TEntity> _crudService;

        public AddOrUpdateEntityCommandHandler(ICrudService<TEntity> crudService)
        {
            _crudService = crudService;
        }

        public Task Handle(AddOrUpdateEntityCommand<TEntity> command)
        {
            _crudService.AddOrUpdate(command.Entity);
            return Task.CompletedTask;
        }
    }
}
