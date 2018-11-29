using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
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
                    Contact = new Contact { Name = "AllenJee", Email = "xiaomaprncess@gmail.com" }
                });
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
