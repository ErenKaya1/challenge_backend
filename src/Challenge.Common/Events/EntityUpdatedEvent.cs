using System;
using Challenge.Core.Entity;

namespace Challenge.Common.Events
{
    public class EntityUpdatedEvent<T> : IDomainEvent where T : BaseEntity<string>
    {
        public EntityUpdatedEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }

        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }
}
