using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.ViewModels
{
    public class UserInfoVM: SystemUser
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public List<string> RoleName { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<MenuInfo> MenuList { get; set; }
    }

    /// <summary>
    /// 菜单数据，根据当前登录角色获取对应的菜单
    /// </summary>
    public class MenuInfo
    {
        public string ModuleName { get; set; }
        public List<SystemMenu> SystemMenus { get; set; }
    }
}
