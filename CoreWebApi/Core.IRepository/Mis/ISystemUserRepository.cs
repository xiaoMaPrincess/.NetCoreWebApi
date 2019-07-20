using Core.Model;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.Mis
{
    public interface ISystemUserRepository: IDependencyMsi
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        Task<PageModel<UserInfoVM>> GetUserList(int pageIndex=1,int pageSize=10 );
    }
}
