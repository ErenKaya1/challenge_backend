using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Common.Services
{
    public interface ICrudService<T> where T : AggregateRoot<string>
    {
        IList<T> Get();
        T GetById(string id);
        Task AddOrUpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(string id, bool triggerEvent = false);
    }
}