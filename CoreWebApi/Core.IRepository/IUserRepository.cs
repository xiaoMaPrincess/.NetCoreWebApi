using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    /// <summary>
    /// 用户数据层接口
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        Task<LoginResult> UserRegister(LoginVM loginVM);
    }
}
