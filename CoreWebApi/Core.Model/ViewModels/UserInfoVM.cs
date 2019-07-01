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
        public string RoleName { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<SystemMenu> SystemMenus { get; set; }
    }

    public class Message
    {

    }
}
