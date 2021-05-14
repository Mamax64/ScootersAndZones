using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
