using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.AOP
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICaching
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
    }
}
