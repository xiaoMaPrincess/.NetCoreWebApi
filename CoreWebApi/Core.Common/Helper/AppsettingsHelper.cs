using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Helper
{
    /// <summary>
    /// appsetting.json操作类
    /// </summary>
    public class AppsettingsHelper
    {
        static IConfiguration Configuration { get; set; }
        static AppsettingsHelper()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                var value = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    value += sections[i] + ":";
                }
                return Configuration[value.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
