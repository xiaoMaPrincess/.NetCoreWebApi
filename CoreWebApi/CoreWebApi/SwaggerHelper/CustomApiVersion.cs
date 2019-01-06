using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.SwaggerHelper
{
    /// <summary>
    /// 自定义接口版本
    /// </summary>
    public class CustomApiVersion
    {
        public enum ApiVersions
        {
            /// <summary>
            /// v1版本
            /// </summary>
            v1=1,
            /// <summary>
            /// v2版本
            /// </summary>
            v2=2,
        }
    }
}
