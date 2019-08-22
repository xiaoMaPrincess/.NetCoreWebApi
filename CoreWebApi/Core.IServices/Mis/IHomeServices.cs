using Core.Model;
using Core.Model.Models;
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
        /// <summary>
        /// 根据用户与访问菜单获取对应的按钮
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="menuID"></param>
        /// <returns></returns>
        Task<List<SystemAction>> GetActionList(Guid userID,Guid menuID);
    }
}
