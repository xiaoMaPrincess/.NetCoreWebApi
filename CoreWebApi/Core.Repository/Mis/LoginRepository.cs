using Core.Common.EFCore;
using Core.IRepository.Mis;
using Core.Model.Models;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Model;
using Core.Common;
using Core.Common.Helper;

namespace Core.Repository.Mis
{
    /// <summary>
    /// 登录数据层实现
    /// </summary>
    public class LoginRepository : ILoginRepository
    {
        private readonly IEFContext _context;
        public LoginRepository(IEFContext context)
        {
            _context = context;
        }
        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name="itCode"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        public ResponseMessage<UserInfoVM> GetUserInfo(string itCode, string password)
        {
            var response = new ResponseMessage<UserInfoVM>();
            if (string.IsNullOrWhiteSpace(itCode))
            {
                response.Message = "请输入用户名";
                response.IsSuccess = false;
                return response;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                response.Message = "请输入密码";
                response.IsSuccess = false;
                return response;
            }

            password = MD5Helper.GetMD5String(password);
            var code = (from a in _context.Set<SystemUser>()
                        where a.ITCode == itCode
                        select new
                        {
                            ID = a.ID
                        }).FirstOrDefault();
            if (code == null)
            {
                response.Message = "用户名不存在";
                response.IsSuccess = false;
                return response;
            }
            var pwd = (from a in _context.Set<SystemUser>()
                       where a.ITCode == itCode && a.Password == password
                       select new
                       {
                           ID = a.ID
                       }).FirstOrDefault();
            if (pwd == null)
            {
                response.Message = "密码错误，请重新输入";
                response.IsSuccess = false;
                return response;
            }

            var data = (from a in _context.Set<SystemUser>()
                        join b in _context.Set<SystemUserRole>()
                        on a.ID equals b.UserID
                        where a.ID == pwd.ID
                        select new
                        {
                            ID = a.ID,
                            UserName = a.Name,
                            RoleID = b.RoleID
                        }).FirstOrDefault();
            if (data != null)
            {
                response.Message = "登录成功，欢迎来到CloudBlog管理平台";
                response.IsSuccess = true;

                response.Data = new UserInfoVM();
                response.Data.ID = data.ID;
                response.Data.Name = data.UserName;
                // 写入session
                return response;
            }
            else
            {
                response.Message = "当前用户未分配角色，请联系管理员";
                response.IsSuccess = false;
                return response;
            }

        }
    }
}
