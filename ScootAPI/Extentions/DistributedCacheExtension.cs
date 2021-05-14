using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ScootAPI.Extentions
{
    public static class DistributedCacheExtension
    {
        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key)
        {
            var jsonData = await distributedCache.GetStringAsync(key);

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}
