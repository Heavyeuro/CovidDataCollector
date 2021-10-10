using CovidDataCollector.Models;
using CovidDataCollector.Serializer;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CovidDataCollector.Extensions
{
    public static class DistributedCacheExtension
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data,
            TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromHours(2),
                SlidingExpiration = unusedExpireTime,
            };

            var jsonData = CovidStatSerializer.SerializeObject(data as BaseCovidStatModel);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

             return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}