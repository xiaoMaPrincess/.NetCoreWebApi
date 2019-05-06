using Core.IRepository;
using Core.IServices;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// 用户业务层实现
    /// </summary>
    public class UserServices : IUserServices
    {
        private readonly IUserRepository userRepository;
        public UserServices(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        public async Task<LoginResult> UserRegister(LoginVM loginVM)
        {
           return await userRepository.UserRegister(loginVM);
        }
    }
}
