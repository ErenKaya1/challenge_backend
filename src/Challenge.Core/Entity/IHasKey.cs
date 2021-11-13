namespace Challenge.Core.Entity
{
    public interface IHasKey<T>
    {
        T Id { get; set; }
    }
}