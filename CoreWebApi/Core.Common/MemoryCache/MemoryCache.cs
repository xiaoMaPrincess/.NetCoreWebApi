using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.MemoryCache
{
    public class Cache : ICache
    {
        private IMemoryCache _cache;

        public Cache(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 根据Key取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="cacheLey"></param>
        /// <param name="cacheValue"></param>
        public void Set(string cacheLey, object cacheValue)
        {
            // 设置过期时间
            _cache.Set(cacheLey, cacheValue, TimeSpan.FromSeconds(7200));
        }
    }
}
