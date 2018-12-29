using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.AOP
{
    /// <summary>
    /// 拦截器
    /// </summary>
    public class CoreLogAOP : IInterceptor
    {
        /// <summary>
        /// 实现IInterceptor方法
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var dataIntercept=$"{DateTime.Now.ToString("yyyyMMDDHHmmss")}"+$"当前执行方法:{invocation.Method.Name}"+$"参数是:{string.Join(",",invocation.Arguments.Select(a=>(a??"")).ToString().ToArray())} \r\n";

            // 执行后，继续执行当前方法
            invocation.Proceed();
            dataIntercept += ($"方法执行完毕，返回结果:{invocation.ReturnValue}");

            #region 输出到当前项目日志
            var path = Directory.GetCurrentDirectory() + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyy-MM-dd HHmmss")}.log";
            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(dataIntercept);
            sw.Close();
            #endregion
        }
    }
}
