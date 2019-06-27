using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.MemoryCache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 取数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        object Get(string cacheKey);
        /// <summary>
        /// 存数据
        /// </summary>
        /// <param name="cacheLey"></param>
        /// <param name="cacheValue"></param>
        void Set(string cacheLey, object cacheValue);
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="cacheKey"></param>
        void Remove(string cacheKey);
    }
}
