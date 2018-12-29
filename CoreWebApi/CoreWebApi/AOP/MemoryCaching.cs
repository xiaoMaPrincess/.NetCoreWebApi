using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.AOP
{
    /// <summary>
    /// 实例化缓存接口
    /// </summary>
    public class MemoryCaching : ICaching
    {

        private IMemoryCache _cache;

        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void Set(string cacheLey, object cacheValue)
        {
            // 设置过期时间
            _cache.Set(cacheLey, cacheValue, TimeSpan.FromSeconds(7200));
        }
    }
}
