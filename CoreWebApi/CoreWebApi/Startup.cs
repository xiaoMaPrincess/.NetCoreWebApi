using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Core.IServices;
using CoreWebApi.AOP;
using CoreWebApi.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // 注入缓存
            services.AddScoped<ICaching, MemoryCaching>();

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

                // 获取应用程序根路径
                var basePath =Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                // 获取Model层的xml文件
                var xmlModelPath = Path.Combine(basePath, "Core.Model.xml");
                // 启用Model层注释
                x.IncludeXmlComments(xmlModelPath);
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

            // 添加角色
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                // 并列（与）
                //options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
                // 或
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            });

            // 添加认证
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience 
                    ValidateIssuerSigningKey = true,//是否验证IssuerSigningKey 
                    ValidIssuer = "Blog.Core",
                    ValidAudience = "wr",
                    ValidateLifetime = true,//是否验证超时  当设置exp和nbf时有效 同时启用ClockSkew 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtHelper.secretKey)),
                    //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.FromSeconds(30)

                };
            });
            #endregion

            // 数据库连接字符串
            //BaseDBConfig.ConnectionString = Configuration.GetSection("ConnectionString:Value").Value;

            #region AutoFac
            // 实例化AutoFac容器
            var builder = new ContainerBuilder();
            // 注册拦截器
            builder.RegisterType<CoreLogAOP>();

            // 注册 单个注册
           // builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

            // 获取项目路径
            var pathBase = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            var servicesDllFile = Path.Combine(pathBase, "Core.Services.dll");// 获取注入项目绝对路径
            // 加载程序集，这里为实现层
            //var assemblysServices = Assembly.Load("Core.Services");
            var assemblysServices = Assembly.LoadFile(servicesDllFile);// 使用加载文件的方法加载程序集
            // 指定已加载程序集中的类型，并注册其所实现的接口
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().
                InstancePerLifetimeScope().
                EnableInterfaceInterceptors(). //添加拦截器到接口或类之上
                InterceptedBy(typeof(CoreLogAOP)); //将服务分配

            var repositoryDLLFile = Path.Combine(pathBase, "Core.Repository.dll");
            var assemblysRepository = Assembly.LoadFile(repositoryDLLFile);
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            // 将services 填充AutoFac容器生成器，使原来ConfigureServices中的服务可用。可以理解为使用Auto接管原来的ConfigureServices
            builder.Populate(services);
            //使用以注册的组件登记穿件新容器
            var applicationContainer= builder.Build();
            #endregion
            // 第三方IOC接管
            return new AutofacServiceProvider(applicationContainer);


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
            // app.UseMiddleware<JwtTokenAuth>();//  此方法已废弃
            // 使用官方授权
            app.UseAuthentication();
            // 添加自定义中间件的第二种方式
           // app.UseRequestCulture();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
