using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices.Mis;
using Core.Model.SearchModels;
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
        [HttpGet]
        public async Task<IActionResult> GetUserList(UserSearch search)
        {
            var data = await _userServices.GetUserList(search);
            return Json(data);
        }

    }
}