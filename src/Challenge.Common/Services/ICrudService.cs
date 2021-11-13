using System.Collections.Generic;

namespace Challenge.Common.Services
{
    public interface ICrudService<T>
        where T : AggregateRoot<string>
    {
        IList<T> Get();

        T GetById(string id);

        void AddOrUpdate(T entity);

        void Delete(T entity);
    }
}
