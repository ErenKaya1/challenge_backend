using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge.Common.Services;
using MongoDB.Driver;

namespace Challenge.Common.Queries
{
    public class GetEntititesQuery<TEntity> : IQuery<List<TEntity>> where TEntity : AggregateRoot<string>
    {
    }

    public class GetEntititesQueryHandler<TEntity> : IQueryHandler<GetEntititesQuery<TEntity>, List<TEntity>> where TEntity : AggregateRoot<string>
    {
        private readonly IRepository<TEntity> _repository;

        public GetEntititesQueryHandler(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<List<TEntity>> Handle(GetEntititesQuery<TEntity> query)
        {
            return await _repository.GetAll().ToListAsync();
        }
    }
}
