using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices.Mis;
using Microsoft.AspNetCore.Mvc;

namespace Core.Msi.Controllers
{
    public class SystemUserController : Controller
    {
        private readonly ISystemUserServices _userServices;
        public SystemUserController(ISystemUserServices userServices)
        {
            _userServices = userServices;
        }
        public IActionResult Index()
        {
            return PartialView();
        }
        
    }
}