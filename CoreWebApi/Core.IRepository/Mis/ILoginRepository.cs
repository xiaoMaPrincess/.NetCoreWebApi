using Core.Model;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.IRepository.Mis
{
   public interface ILoginRepository: IDependencyMsi
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="itCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ResponseMessage<UserInfoVM> GetUserInfo(string itCode, string password);
    }
}
