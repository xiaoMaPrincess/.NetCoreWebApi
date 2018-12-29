using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.AOP
{
    /// <summary>
    /// AOP 缓存
    /// </summary>
    public class CoreCacheAOP : IInterceptor
    {
        private ICaching _cache;
        public CoreCacheAOP(ICaching cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            // 获取自定缓存键
            var cacheKey = CustomCacheKey(invocation);

            var cacheValue = _cache.Get(cacheKey);
            if (cacheValue != null)
            {
                // 获取当前的缓存值，并作为返回值返回
                invocation.ReturnValue = cacheValue;
                return;
            }
            // 继续执行当前请求
            invocation.Proceed();

            // 存入缓存
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                _cache.Set(cacheKey, invocation.ReturnValue);
            }


        }

        /// <summary>
        /// 自定义缓存键
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();
            string key = $"{typeName}:{methodName}:";
            foreach (var item in methodArguments)
            {
                key += $"{ item}";
            }

            return key.TrimEnd(':');
        }

        /// <summary>
        /// object 转 stirng
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string GetArgumentValue(object arg)
        {
            if(arg is int||arg is long ||arg is string)
            {
                return arg.ToString();
            }
            if(arg is DateTime)
            {
                return ((DateTime)arg).ToString("yyyy-MM-DD HH:mm:ss");
            }
            return "";
        }
    }
}
