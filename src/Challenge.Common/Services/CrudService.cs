using System;
using System.Collections.Generic;
using System.Linq;
using Challenge.Common.Events;

namespace Challenge.Common.Services
{
    public class CrudService<T> : ICrudService<T>
        where T : AggregateRoot<string>
    {
        protected readonly IRepository<T> _repository;
        private readonly IDomainEvents _domainEvents;

        public CrudService(IRepository<T> repository, IDomainEvents domainEvents)
        {
            _repository = repository;
            _domainEvents = domainEvents;
        }

        public virtual void AddOrUpdate(T entity)
        {
            var adding = string.IsNullOrWhiteSpace(entity.Id);
            
            if (adding)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                _repository.Add(entity);
                
                _domainEvents.Dispatch(new EntityCreatedEvent<T>(entity, DateTime.UtcNow));
            }
            else
            {
                entity.UpdatedDateTime = DateTime.UtcNow;
                _repository.Update(entity);

                _domainEvents.Dispatch(new EntityUpdatedEvent<T>(entity, DateTime.UtcNow));
            }
        }

        public virtual IList<T> Get()
        {
            return _repository.GetAll().ToList();
        }

        public virtual T GetById(string Id)
        {
            //ValidationException.Requires(Id != Guid.Empty, "Invalid Id");
            return _repository.GetAll().FirstOrDefault(x => x.Id == Id);
        }

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
            _domainEvents.Dispatch(new EntityDeletedEvent<T>(entity, DateTime.UtcNow));
        }
    }
}
