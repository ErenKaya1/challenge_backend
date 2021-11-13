using System.Threading.Tasks;
using Challenge.Common.Services;

namespace Challenge.Common.Queries
{
    public class GetEntityByIdQuery<TEntity> : IQuery<TEntity> where TEntity : AggregateRoot<string>
    {
        public string Id { get; set; }
    }

    public class GetEntityByIdQueryHandler<TEntity> : IQueryHandler<GetEntityByIdQuery<TEntity>, TEntity> where TEntity : AggregateRoot<string>
    {
        private readonly IRepository<TEntity> _repository;

        public GetEntityByIdQueryHandler(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<TEntity> Handle(GetEntityByIdQuery<TEntity> query)
        {
            var entity = await _repository.GetByIdAsync(query.Id);

            if (entity == null)
                throw new System.Exception($"Entity {query.Id} not found.");

            return entity;
        }
    }
}