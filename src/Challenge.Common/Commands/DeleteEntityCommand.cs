using System.Threading.Tasks;
using Challenge.Common.Services;

namespace Challenge.Common.Commands
{
    public class DeleteEntityCommand<TEntity> : ICommand
         where TEntity : AggregateRoot<string>
    {
        public TEntity Entity { get; set; }
    }

    public class DeleteEntityCommandHandler<TEntity> : ICommandHandler<DeleteEntityCommand<TEntity>>
    where TEntity : AggregateRoot<string>
    {
        private readonly ICrudService<TEntity> _crudService;

        public DeleteEntityCommandHandler(ICrudService<TEntity> crudService)
        {
            _crudService = crudService;
        }

        public Task Handle(DeleteEntityCommand<TEntity> command)
        {
            _crudService.Delete(command.Entity);
            return Task.CompletedTask;
        }
    }
}
