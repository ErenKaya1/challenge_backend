namespace Challenge.Common.Events
{
    public interface IDomainEvents
    {
        void Dispatch(IDomainEvent domainEvent);
    }
}
