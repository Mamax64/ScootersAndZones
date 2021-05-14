using System.Collections.Generic;

namespace Cache
{
    public interface IRedisService
    {
        T Get<T>(string key) where T : class;
        void Set<T>(string key, T value) where T : class;
        void Delete(string key);
        List<string> GetKeysByPattern(string pattern);
    }
}
