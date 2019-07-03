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

namespace Core.Msi.Controllers
{
    [ValidateUserFilter]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            var user = UserHelper.GetUserInfo();
            return View(user);
        }

        public IActionResult Header()
        {
            return View();
        }

        public IActionResult Menu()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
