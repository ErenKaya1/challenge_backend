using System;
using Challenge.Core.Entity;

namespace Challenge.Common.Events
{
    public class EntityDeletedEvent<T> : IDomainEvent where T : BaseEntity<string>
    {
        public EntityDeletedEvent(T entity, DateTime eventDateTime)
        {
            Entity = entity;
            EventDateTime = eventDateTime;
        }

        public T Entity { get; }

        public DateTime EventDateTime { get; }
    }
}
