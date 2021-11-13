using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Core.Settings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static Challenge.Core.Enums.Enums;

namespace Challenge.Common.Services
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : AggregateRoot<string>
    {
        private readonly IMongoCollection<TEntity> mongoCollection;
        private readonly MongoClient client;

        public BaseRepository(MongoDBSettings settings)
        {
            client = new MongoClient(settings.ConnectionString);

            var db = client.GetDatabase(settings.DatabaseName);
            mongoCollection = db.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public IMongoCollection<TEntity> GetCollection()
        {
            return mongoCollection;
        }

        public IMongoQueryable<TEntity> GetAll()
        {
            return mongoCollection.AsQueryable().Where(x => x.RecordStatus != RecordStatus.Deleted);
        }

        public async Task<List<TEntity>> GetAllAsyncToList()
        {
            return await mongoCollection.AsQueryable().Where(x => x.RecordStatus != RecordStatus.Deleted).ToListAsync();
        }


        public IMongoQueryable<TEntity> GetBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted);
        }

        public async Task<List<TEntity>> GetByToListAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted).ToListAsync();
        }

        public bool Any(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted).Any();
        }

        public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted).AnyAsync();
        }

        public int Count(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted).Count();
        }

        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted).CountAsync();
        }

        public TEntity GetById(string Id)
        {
            return mongoCollection.Find(x => x.Id == Id && x.RecordStatus != RecordStatus.Deleted).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetByIdAsync(string Id)
        {
            return await mongoCollection.Find(x => x.Id == Id && x.RecordStatus != RecordStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public TEntity FirstOrDefaultBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted).FirstOrDefault();
        }

        public async Task<TEntity> FirstOrDefaultByAsync(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RecordStatus != RecordStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public virtual TEntity Add(TEntity entity, bool forceDates = false)
        {
            if (!forceDates)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = DateTime.UtcNow;
            }


            entity.RecordStatus = Core.Enums.Enums.RecordStatus.Active;
            mongoCollection.InsertOne(entity);
            return entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, bool forceDates = false)
        {
            if (!forceDates)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = DateTime.UtcNow;
            }

            entity.RecordStatus = Core.Enums.Enums.RecordStatus.Active;
            await mongoCollection.InsertOneAsync(entity);
            return entity;
        }

        public virtual List<TEntity> Add(List<TEntity> entities, bool forceDates = false)
        {
            if (!forceDates)
            {
                entities.ForEach(x =>
                {
                    x.CreatedDateTime = DateTime.UtcNow;
                    x.UpdatedDateTime = DateTime.UtcNow;
                    x.RecordStatus = Core.Enums.Enums.RecordStatus.Active;
                });
            }

            mongoCollection.InsertMany(entities);
            return entities;
        }

        public virtual async Task<List<TEntity>> AddAsync(List<TEntity> entities)
        {
            entities.ForEach(x =>
            {
                x.CreatedDateTime = DateTime.UtcNow;
                x.UpdatedDateTime = DateTime.UtcNow;
                x.RecordStatus = Core.Enums.Enums.RecordStatus.Active;
            });
            await mongoCollection.InsertManyAsync(entities);
            return entities;
        }

        public virtual void Update(TEntity entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            mongoCollection.ReplaceOne(m => m.Id == entity.Id, entity);
        }

        public virtual async Task UpdateAsync(TEntity entity, bool forceDates = false)
        {
            if (!forceDates)
                entity.UpdatedDateTime = DateTime.UtcNow;

            await mongoCollection.ReplaceOneAsync(m => m.Id == entity.Id, entity);
        }

        public virtual void Update(string id, TEntity entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            mongoCollection.ReplaceOne(m => m.Id == id, entity);
        }

        public virtual async Task UpdateAsync(string id, TEntity entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
            await mongoCollection.ReplaceOneAsync(m => m.Id == id, entity);
        }

        public virtual void Delete(TEntity entity)
        {
            entity.RecordStatus = Core.Enums.Enums.RecordStatus.Deleted;
            entity.UpdatedDateTime = DateTime.UtcNow;
            this.Update(entity.Id, entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            entity.RecordStatus = Core.Enums.Enums.RecordStatus.Deleted;
            entity.UpdatedDateTime = DateTime.UtcNow;
            await this.UpdateAsync(entity.Id, entity);
        }

        public virtual void Delete(string id)
        {
            var entity = this.GetById(id);
            this.Delete(entity);
        }

        public virtual async Task DeleteAsync(string id)
        {
            var entity = await this.GetByIdAsync(id);
            await this.DeleteAsync(entity);
        }

        public virtual DeleteResult HardDelete(string id)
        {
            return mongoCollection.DeleteOne(Builders<TEntity>.Filter.Eq(x => x.Id, id));
        }

        public virtual async Task<DeleteResult> HardDeleteAsync(string id)
        {
            return await mongoCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq(x => x.Id, id));
        }

        public virtual DeleteResult HardDeleteMany(FilterDefinition<TEntity> filter)
        {
            return mongoCollection.DeleteMany(filter);
        }

        public virtual async Task<DeleteResult> HardDeleteManyAsync(FilterDefinition<TEntity> filter)
        {
            return await mongoCollection.DeleteManyAsync(filter);
        }

        public virtual UpdateResult UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update,
            bool updateDate = true)
        {
            if (updateDate)
                update = update.Set(x => x.UpdatedDateTime, DateTime.UtcNow);

            return mongoCollection.UpdateMany(filter, update);
        }

        public virtual async Task<UpdateResult> UpdateManyAsync(FilterDefinition<TEntity> filter,
            UpdateDefinition<TEntity> update, bool updateDate = true)
        {
            if (updateDate)
                update = update.Set(x => x.UpdatedDateTime, DateTime.UtcNow);

            return await mongoCollection.UpdateManyAsync(filter, update);
        }

        public virtual UpdateResult DeleteMany(FilterDefinition<TEntity> filter)
        {
            var update = Builders<TEntity>.Update
                .Set(x => x.RecordStatus, RecordStatus.Deleted)
                .Set(x => x.UpdatedDateTime, DateTime.UtcNow);

            return mongoCollection.UpdateMany(filter, update);
        }

        public virtual async Task<UpdateResult> DeleteManyAsync(FilterDefinition<TEntity> filter)
        {
            var update = Builders<TEntity>.Update
                .Set(x => x.RecordStatus, RecordStatus.Deleted)
                .Set(x => x.UpdatedDateTime, DateTime.UtcNow);

            return await mongoCollection.UpdateManyAsync(filter, update);
        }
    }
}