using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Linq;

namespace Cache
{
    public class RedisService : IRedisService
    {
        private readonly IConfiguration _configuration;
        private readonly RedisEndpoint _redisConfiguration;

        public RedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisConfiguration = new RedisEndpoint() { Host = _configuration["Redis:Host"], Password = _configuration["Redis:Password"], Port = _configuration["Redis:Port"].ToInt() };
        }

        public void Set<T>(string key, T value) where T : class
        {
            using IRedisClient client = new RedisClient(_redisConfiguration);
            client.Set(key, value);
        }

        public T Get<T>(string key) where T : class
        {
            using IRedisClient client = new RedisClient(_redisConfiguration);
            return client.Get<T>(key);
        }

        public void Delete(string key)
        {
            using IRedisClient client = new RedisClient(_redisConfiguration);
            client.Remove(key);
        }

        public List<string> GetKeysByPattern(string pattern)
        {
            using IRedisClient client = new RedisClient(_redisConfiguration);
            
            return client.GetKeysByPattern(pattern).ToList();
        }
    }
}
