using Core.Common.Helper;
using Core.Model.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Msi.Common
{
    public class UserHelper
    {
        /// <summary>
        /// 写入登录缓存
        /// </summary>
        /// <param name="userInfoVM">用户信息</param>
        public async static void SetUserInfo(UserInfoVM userInfoVM)
        {
            var user = new ClaimsPrincipal(
              new ClaimsIdentity(new[]
              {
                    new Claim(ClaimTypes.Name,userInfoVM.ID.ToString()),
                    new Claim("UserName",userInfoVM.Name)
              },
              CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContextHelper.Current.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties
    {
        IsPersistent = true,
        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)//Add(TimeSpan.FromHours(2)) // Cookie 有效时间
    }
    );
        }

        /// <summary>
        /// 清除用户信息
        /// </summary>
        public static void ClearUserInfo()
        {
            HttpContextHelper.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfoVM GetUserInfo()
        {
            // LogHelper.Info("2获取登录信息："  );
            if (!HttpContextHelper.Current.User.Identity.IsAuthenticated)
            {
                //LogHelper.Info("2.1获取登录信息：IsAuthenticated::"+ HttpContext.Current.User.Identity.Name);
                return null;
            }
            UserInfoVM user = new UserInfoVM();
            user.ID = new Guid(HttpContextHelper.Current.User.Identity.Name);
            user.Name = HttpContextHelper.Current.User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
            return user;
        }
    }
}
