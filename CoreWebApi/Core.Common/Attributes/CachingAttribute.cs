using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Attributes
{
    /// <summary>
    /// 使用时验证，添加到要缓存数据的方法中。针对Method有效
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,Inherited =true)]
    public class CachingAttribute: Attribute
    {
        // 缓存绝对过期时间
        public int AbsoluteExpiration { get; set; } = 30;
    }
}
