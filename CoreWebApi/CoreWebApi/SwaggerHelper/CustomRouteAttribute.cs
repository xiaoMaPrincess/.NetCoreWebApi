using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoreWebApi.SwaggerHelper.CustomApiVersion;

namespace CoreWebApi.SwaggerHelper
{
    /// <summary>
    /// 自定义路由 /api/{version}/[controler]/[action]
    /// </summary>
    public class CustomRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {
        /// <summary>
        /// 分组名称，用来实现实现IApiDescriptionGroupNameProvider
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 自定义路由构造函数，继承基类路由
        /// </summary>
        /// <param name="actionName"></param>
        public CustomRouteAttribute(string actionName = "[action]"):base("/api/{version}/[controller]/"+actionName)
        {

        }

        /// <summary>
        /// 自定义路由构造函数，继承基类路由
        /// </summary>
        /// <param name="actionName">方法名</param>
        /// <param name="version">版本</param>
        public CustomRouteAttribute(ApiVersions version, string actionName = "[action]") : base($"/api/{version.ToString()}/[controller]/{actionName}")
        {
            GroupName = version.ToString();
        }

    }
}
