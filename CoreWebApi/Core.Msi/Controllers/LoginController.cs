using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Common;
using Core.Common.Helper;
using Core.IServices.Mis;
using Core.Msi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Core.Msi.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginServices _loginServices;
        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="itCode">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(string itCode, string password)
        {
            var result = _loginServices.GetUserInfo(itCode, password);
            if (result.IsSuccess)
            {
                // 写入登录缓存
                UserHelper.SetUserInfo(result.Data);
            }
            return Json(result);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout(Guid id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            UserHelper.ClearUserInfo();
            return RedirectToAction("Login", "Login");

        }
    }
}