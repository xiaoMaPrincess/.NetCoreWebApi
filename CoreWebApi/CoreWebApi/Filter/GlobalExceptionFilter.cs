using CoreWebApi.Log;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Filter
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILoggerHelper _loggerHelper;
        public GlobalExceptionFilter(IHostingEnvironment env, ILoggerHelper loggerHelper)
        {
            _env = env;
            _loggerHelper = loggerHelper;
        }
        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            // 错误信息
            json.Message = context.Exception.Message;
            if (_env.IsDevelopment())
            {
                // 开发环境，记录堆栈信息
                json.DevelopmentMessage = context.Exception.StackTrace;
            }
            // 使用log4net记录错误日志
            context.Result = new InternalServerErrorObjectResult(json);
            _loggerHelper.Error(json.Message, WriteLog(json.Message, context.Exception));
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string WriteLog(string throwMsg,Exception ex)
        {
            return string.Format("【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

    /// <summary>
    /// 返回错误信息
    /// </summary>
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 开发环境消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }
}
