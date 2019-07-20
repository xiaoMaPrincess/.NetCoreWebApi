using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Msi.Models;
using Core.Common.EFCore;
using Core.Model.Models;
using Core.Msi.Common;
using Core.Msi.Filter;
using Core.IServices.Mis;

namespace Core.Msi.Controllers
{
    [ValidateUserFilter]
    public class HomeController : Controller
    {

        private readonly IHomeServices _homeServices;
        public HomeController(IHomeServices homeServices)
        {
            _homeServices = homeServices;
        }
        public async Task<IActionResult> Index()
        {
            var user = UserHelper.GetUserInfo();
            user.MenuList=await _homeServices.GetMenuList(user.ID);
            return View(user);
        }

        /// <summary>
        /// 主页面头部
        /// </summary>
        /// <returns></returns>
        public IActionResult Header()
        {
            return View();
        }
        /// <summary>
        /// 左侧菜单
        /// </summary>
        /// <returns></returns>
        public IActionResult Menu()
        {
            return View();
        }

        /// <summary>
        /// 首页图表展示
        /// </summary>
        /// <returns></returns>
        public IActionResult ECharts()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            if (statusCode==404)
            {
                return View("_404");
            }
            return View();
        }
    }
}
