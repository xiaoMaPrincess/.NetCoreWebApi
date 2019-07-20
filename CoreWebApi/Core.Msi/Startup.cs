using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Common;
using Core.Common.EFCore;
using Core.Common.MemoryCache;
using Core.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Msi
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 添加 Cook 服务
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // 启用缓存服务
            services.AddMemoryCache();

            // 注入自定义缓存接口
            services.AddScoped<ICache, Cache>();

            // 启用session
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = false;
            });
            // UserHleper
            //services.AddScoped<IUserHelper, UserHelper>();

            // 注入EF
            services.AddScoped<IEFContext, EFContext>();
            // 启用EFCore服务
            services.AddDbContext<EFContext>(options =>
    options.UseMySql(Configuration.GetConnectionString("ConnectionString")));
            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            #region AutoFac
            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            // 获取项目路径
            var pathBase = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            // 获取dll文件的绝对路径
            var servicesDllFile = Path.Combine(pathBase, "Core.Services.dll");
            var repositoryDllFile = Path.Combine(pathBase, "Core.Repository.dll");

            // 加载程序集，这里为实现层
            var assemblyRepository = Assembly.LoadFile(repositoryDllFile);
            var assemblysServices = Assembly.LoadFile(servicesDllFile);

            // 依赖注入接口
            var baseType = typeof(IDependencyMsi);

            // 批量注入
            containerBuilder.RegisterAssemblyTypes(assemblysServices).Where(x => baseType.IsAssignableFrom(x) && x != baseType).AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterAssemblyTypes(assemblyRepository).Where(x => baseType.IsAssignableFrom(x) && x != baseType).AsImplementedInterfaces().InstancePerLifetimeScope();
            //// 批量注入
            //containerBuilder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();

            //containerBuilder.RegisterAssemblyTypes(assemblyRepository).AsImplementedInterfaces();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            #endregion

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            // 状态码中间件
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseSession();
            // cookie中间件
            app.UseCookiePolicy();
            // 验证中间件
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
           Core.Common.Helper.HttpContextHelper.ServiceProvider = app.ApplicationServices;
        }
    }
}
