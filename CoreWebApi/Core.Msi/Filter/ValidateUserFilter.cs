using Core.Msi.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Core.Msi.Filter
{
    public class ValidateUserFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateUserStatus(context);
            base.OnActionExecuting(context);
        }


        /// <summary>
        /// 验证是否处于登录状态
        /// </summary>
        /// <param name="filterContext"></param>
        private void ValidateUserStatus(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            // 获取正在访问的url
            var url = string.Concat(request.Path, request.QueryString);
            string backUrl= HttpUtility.UrlEncode(url);

            var user = UserHelper.GetUserInfo();
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }

        }
    }
}
