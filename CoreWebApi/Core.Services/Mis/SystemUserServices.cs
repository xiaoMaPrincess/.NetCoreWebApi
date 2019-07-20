using Core.IRepository.Mis;
using Core.IServices.Mis;
using Core.Model;
using Core.Model.Models;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Mis
{
    public class SystemUserServices : ISystemUserServices
    {

        private readonly ISystemUserRepository _userRepository;
        public SystemUserServices(ISystemUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PageModel<UserInfoVM>> GetUserList(int pageIndex = 1, int pageSize = 10)
        {
           return await _userRepository.GetUserList(pageIndex, pageSize);
        }
    }
}