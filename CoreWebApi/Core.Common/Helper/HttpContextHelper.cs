using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Helper
{
    public static class HttpContextHelper
    {
        public static IServiceProvider ServiceProvider;

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {

                //object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor));
                //Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Mvc.Infrastructure.ActionContextAccessor)factory).ActionContext.HttpContext;

                object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;

                return context;
            }
        }

    }
}
