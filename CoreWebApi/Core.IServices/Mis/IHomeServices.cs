using Core.Model;
using Core.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.Mis
{
    public interface IHomeServices : IDependencyMsi
    {
        /// <summary>
        /// 获取当前用户所用的菜单列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<List<MenuInfo>> GetMenuList(Guid userID);
    }
}
