using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.EFCore;
using Core.Model.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Msi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceContext = services.GetRequiredService<IConfiguration>();
                    var connectionString = serviceContext.GetValue<string>("ConnectionString");
                    var context = new EFContext(connectionString, Model.DBTypeEnum.Mysql);
                    List<SystemModule> list = new List<SystemModule>() { new SystemModule {ModuleName="用户管理",ClassName= "SystemUser"}, new SystemModule { ModuleName = "角色管理", ClassName = "SystemRole" }, new SystemModule { ModuleName = "用户组管理", ClassName = "SystemGroup" }, new SystemModule { ModuleName = "菜单管理", ClassName = "SystemMenu" } };
                    // 初始化数据
                     context.DataInit(list);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
