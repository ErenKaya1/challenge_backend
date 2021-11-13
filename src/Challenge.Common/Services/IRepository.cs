using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Challenge.Common.Services
{
    public interface IRepository<TEntity> where TEntity : AggregateRoot<string>
    {
        IMongoCollection<TEntity> GetCollection();
        IMongoQueryable<TEntity> GetAll();
        IMongoQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> expression);

        bool Any(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

        int Count(Expression<Func<TEntity, bool>> expression);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);

        TEntity GetById(string Id);
        Task<TEntity> GetByIdAsync(string Id);

        TEntity FirstOrDefaultBy(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FirstOrDefaultByAsync(Expression<Func<TEntity, bool>> expression);

        TEntity Add(TEntity entity, bool forceDates = false);
        Task<TEntity> AddAsync(TEntity entity, bool forceDates = false);

        List<TEntity> Add(List<TEntity> entities, bool forceDates = false);
        Task<List<TEntity>> AddAsync(List<TEntity> entities);

        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity, bool forceDates = false);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);

        void Delete(string id);
        Task DeleteAsync(string id);

        DeleteResult HardDelete(string id);
        Task<DeleteResult> HardDeleteAsync(string id);

        DeleteResult HardDeleteMany(FilterDefinition<TEntity> filter);
        Task<DeleteResult> HardDeleteManyAsync(FilterDefinition<TEntity> filter);

        UpdateResult UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, bool updateDate = true);
        Task<UpdateResult> UpdateManyAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, bool updateDate = true);

        UpdateResult DeleteMany(FilterDefinition<TEntity> filter);
        Task<UpdateResult> DeleteManyAsync(FilterDefinition<TEntity> filter);
        Task<List<TEntity>> GetAllAsyncToList();
        Task<List<TEntity>> GetByToListAsync(Expression<Func<TEntity, bool>> expression);
    }

}
