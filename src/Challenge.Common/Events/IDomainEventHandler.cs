namespace Challenge.Common.Events
{
    public interface IDomainEventHandler<T>
           where T : IDomainEvent
    {
        void Handle(T domainEvent);
    }
}
