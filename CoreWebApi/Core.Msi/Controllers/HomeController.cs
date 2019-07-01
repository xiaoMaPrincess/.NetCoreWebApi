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

namespace Core.Msi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEFContext _eFContext;
        public HomeController(IEFContext eFContext)
        {
            _eFContext = eFContext;
        }
        public IActionResult Index()
        {
            var obj= _eFContext.Set<SystemUser>().FirstOrDefault();
            var msg=  UserHelper.GetUserInfo();
            if (obj != null)
            {
                ViewBag.User = obj.Name;
                if (msg != null)
                {
                    ViewBag.Name = msg.Name;
                    ViewBag.ID = msg.ID;
                }

            }
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
