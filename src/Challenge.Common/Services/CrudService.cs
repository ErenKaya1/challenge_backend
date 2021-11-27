using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.Common.Events;

namespace Challenge.Common.Services
{
    public class CrudService<T> : ICrudService<T> where T : AggregateRoot<string>
    {
        protected readonly IRepository<T> _repository;
        private readonly IDomainEvents _domainEvents;

        public CrudService(IRepository<T> repository, IDomainEvents domainEvents)
        {
            _repository = repository;
            _domainEvents = domainEvents;
        }

        public virtual async Task AddOrUpdateAsync(T entity)
        {
            var adding = string.IsNullOrWhiteSpace(entity.Id);
            if (adding)
            {
                await _repository.AddAsync(entity);
                _domainEvents.Dispatch(new EntityCreatedEvent<T>(entity, DateTime.UtcNow));
            }
            else
            {
                await _repository.UpdateAsync(entity);
                _domainEvents.Dispatch(new EntityUpdatedEvent<T>(entity, DateTime.UtcNow));
            }
        }

        public virtual IList<T> Get()
        {
            return _repository.GetAll().ToList();
        }

        public virtual T GetById(string Id)
        {
            return _repository.FirstOrDefaultBy(x => x.Id == Id);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity.Id);
            _domainEvents.Dispatch(new EntityDeletedEvent<T>(entity, DateTime.UtcNow));
        }

        public async Task DeleteAsync(string id, bool triggerEvent = false)
        {
            if (triggerEvent)
            {
                var entity = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(entity.Id);
                _domainEvents.Dispatch(new EntityDeletedEvent<T>(entity, DateTime.UtcNow));
            }
            else
                await _repository.DeleteAsync(id);
        }
    }
}