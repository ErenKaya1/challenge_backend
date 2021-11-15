using System;
using System.Threading.Tasks;

namespace Challenge.Application.Services.Cache.Redis
{
    public interface IRedisService
    {
        T Get<T>(string key);
        void Add<T>(string key, T value) where T : class;
        void Add<T>(string key, T value, TimeSpan timeout) where T : class;
        void Add<T>(string key, T value, DateTime absoluteExpiration) where T : class;
        void Add<T>(string key, T value, int cacheTime) where T : class;
        bool IsSet(string key);
        void Remove(string key);
        Task<T> GetAsync<T>(string key);
        Task AddAsync<T>(string key, T value) where T : class;
        Task AddAsync<T>(string key, T value, TimeSpan timeout) where T : class;
        Task AddAsync<T>(string key, T value, DateTime absoluteExpiration) where T : class;
        Task AddAsync<T>(string key, T value, int cacheTime) where T : class;
        Task<bool> IsSetAsync(string key);
        Task RemoveAsync(string key);
    }
}