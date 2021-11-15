using System;
using System.Threading.Tasks;

namespace Challenge.Application.Services.Cache.Redis
{
    public static class RedisExtentions
    {
        private static readonly object _lockObj = new object();

        public static T Get<T>(this IRedisService cacheService, string key, Func<T> acquire) where T : class
        {
            return Get(cacheService, key, 10 * 60, acquire);
        }

        public static T Get<T>(this IRedisService cacheService, string key, int cacheTime, Func<T> acquire) where T : class
        {
            lock (_lockObj)
            {
                if (cacheService.IsSet(key))
                {
                    return cacheService.Get<T>(key);
                }
                else
                {
                    var result = acquire();

                    if (result != null)
                        cacheService.Add<T>(key, result, TimeSpan.FromSeconds(cacheTime));

                    return result;
                }
            }
        }

        public static async Task<T> GetAsync<T>(this IRedisService cacheService, string key, int cacheTime, Func<Task<T>> acquire) where T : class
        {
            if (await cacheService.IsSetAsync(key))
                return await cacheService.GetAsync<T>(key);
            else
            {
                var result = await acquire();

                if (result != null)
                    await cacheService.AddAsync<T>(key, result, TimeSpan.FromSeconds(cacheTime));

                return result;
            }
        }
    }
}