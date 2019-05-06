using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    /// <summary>
    /// 用户业务层接口
    /// </summary>
    public interface IUserServices
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        Task<LoginResult> UserRegister(LoginVM loginVM);
    }
}
