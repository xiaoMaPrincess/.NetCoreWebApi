using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreWebApi.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region 注册Swagger服务
            services.AddSwaggerGen(x =>
            {
                // 文档描述
                x.SwaggerDoc("v1", new Info
                {
                    Version = "v1.0",
                    Title = "Core API",
                    Description = "接口说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "AllenJee", Email = "xiaomaprincess@gmail.com" }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //生成xml文件名
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);// 获取路径
                x.IncludeXmlComments(xmlPath, true);// 启用xml注释，第二个参数是控制器的注释，默认为false

                #region Token绑定到ConfigureServices
                // 添加header验证信息
                var security = new Dictionary<string, IEnumerable<string>> { { "Blog.Core", new string[] { } }, };
                // 添加安全要求
                x.AddSecurityRequirement(security);
                // 定义方案
                x.AddSecurityDefinition("Blog.Core", new ApiKeyScheme
                {
                    Description = "JWT授权： 直接在下框中输入Bearer + 空格 +{token}\"",
                    Name = "Authorization",// JWT默认的参数名称
                    In = "header",// jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });
            #endregion

            #region Token服务注册
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region 将Swagger配置在开发环境，生产环境禁用
                //app.UseSwagger();
                //app.UseSwaggerUI(x =>
                //{
                //    x.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                //});
                #endregion
            }
            else
            {
                app.UseHsts();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });
            #endregion
            // 添加中间件
            app.UseMiddleware<JwtTokenAuth>();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
