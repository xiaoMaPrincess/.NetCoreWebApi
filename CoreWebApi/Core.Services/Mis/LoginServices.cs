using Core.IRepository.Mis;
using Core.IServices.Mis;
using Core.Model;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Mis
{
    public class LoginServices : ILoginServices
    {
        private readonly ILoginRepository _loginRepository;
        public LoginServices(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public ResponseMessage<UserInfoVM> GetUserInfo(string itCode, string password)
        {
           return _loginRepository.GetUserInfo(itCode, password);
        }
    }
}
