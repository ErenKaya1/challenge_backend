using System;
using Challenge.Core.Entity;

namespace Challenge.Common.Events
{
    public class EntityCreatedEvent<T> : IDomainEvent where T : BaseEntity<string>
    {
        public EntityCreatedEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }

        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }
}
