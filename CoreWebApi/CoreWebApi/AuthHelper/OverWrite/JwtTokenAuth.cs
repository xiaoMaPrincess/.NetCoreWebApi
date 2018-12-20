using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.AuthHelper.OverWrite
{
    public class JwtTokenAuth
    {

        private readonly RequestDelegate _next;

        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 处理http请求
        /// </summary>
        /// <param name="httpContext">http请求上下文</param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            // 检测是否包含'Authorization'请求头
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }

            // 获取token 并去除Bearer
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ","");

            TokenModelJWT tm = JwtHelper.SerializeJWT(tokenHeader); // 序列化token，获取授权

            // 授权  可以是多个角色
            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);
            claimList.Add(claim);
            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            return _next(httpContext);
        }
    }

    //public static class RequestCultureMiddlewareExtensions
    //{
    //    public static IApplicationBuilder UseRequestCulture(
    //        this IApplicationBuilder builder)
    //    {
    //        return builder.UseMiddleware<JwtTokenAuth>();
    //    }
    //}
}
