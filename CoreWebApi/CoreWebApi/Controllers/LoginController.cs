using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Policy ="Admin")]
    public class LoginController : Controller
    {
        [HttpGet]
        public JsonResult GetJWTStr(long id = 1, string sub = "Admin")
        {
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            TokenModelJWT tokenModel = new TokenModelJWT();
            tokenModel.Uid = id;
            tokenModel.Role = sub;

            string jwtStr = JwtHelper.IssueJWT(tokenModel);
            return Json(jwtStr);
        }
    }
}