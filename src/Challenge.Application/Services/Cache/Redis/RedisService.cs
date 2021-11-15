using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Challenge.Application.Services.Cache.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cacheService;

        public RedisService(IDistributedCache cacheService)
        {
            _cacheService = cacheService;
        }

        public T Get<T>(string key)
        {
            try
            {
                var tempCacheItem = _cacheService.GetString(key);
                if (tempCacheItem != null)
                    return JsonConvert.DeserializeObject<T>(tempCacheItem);
            }
            catch
            {

            }

            return default(T);
        }

        public void Add<T>(string key, T value, TimeSpan timeout) where T : class
        {
            try
            {
                if (value == null)
                    return;
                _cacheService.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddSeconds(timeout.TotalSeconds)),
                });
            }
            catch
            {

            }
        }

        public void Add<T>(string key, T value, int cacheTime) where T : class
        {
            if (value == null)
                return;
            try
            {
                _cacheService.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddSeconds(cacheTime)),
                });
            }
            catch
            {

            }
        }

        public void Add<T>(string key, T value) where T : class
        {
            if (value == null)
                return;

            try
            {
                _cacheService.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {

                });
            }
            catch
            {

            }
        }

        public bool IsSet(string key)
        {
            try
            {
                var value = _cacheService.GetString(key);
                if (!string.IsNullOrWhiteSpace(value))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public void Remove(string key)
        {
            _cacheService.Remove(key);
        }

        public void Add<T>(string key, T value, DateTime absoluteExpiration) where T : class
        {
            try
            {
                if (value == null)
                    return;
                _cacheService.SetString(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(absoluteExpiration),
                });
            }
            catch
            {

            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var tempCacheItem = await _cacheService.GetStringAsync(key);
                if (tempCacheItem != null)
                {
                    return JsonConvert.DeserializeObject<T>(tempCacheItem);
                }
            }
            catch
            {

            }
            return default(T);
        }

        public async Task AddAsync<T>(string key, T value, TimeSpan timeout) where T : class
        {
            try
            {
                if (value == null)
                    return;

                await _cacheService.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddSeconds(timeout.TotalSeconds)),
                });
            }
            catch
            {
                
            }

            return;
        }

        public async Task AddAsync<T>(string key, T value, int cacheTime) where T : class
        {
            if (value == null)
                return;
            try
            {
                await _cacheService.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddSeconds(cacheTime)),
                });
            }
            catch
            {

            }

            return;
        }

        public async Task AddAsync<T>(string key, T value) where T : class
        {
            if (value == null)
                return;

            try
            {
                await _cacheService.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {

                });
            }
            catch
            {

            }

            return;
        }

        public async Task<bool> IsSetAsync(string key)
        {
            try
            {
                var value = await _cacheService.GetStringAsync(key);
                if (!string.IsNullOrWhiteSpace(value))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _cacheService.RemoveAsync(key);
        }

        public async Task AddAsync<T>(string key, T value, DateTime absoluteExpiration) where T : class
        {
            try
            {
                if (value == null)
                    return;

                await _cacheService.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = new DateTimeOffset(absoluteExpiration),
                });
            }
            catch
            {

            }
            return;
        }
    }
}