using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.ViewModels
{
    public class HomeVM
    {
        /// <summary>
        /// 用户
        /// </summary>
        public UserInfoVM UserInfoVM { get; set; }
        /// <summary>
        /// 当前用户所拥有的菜单
        /// </summary>
        public List<SystemMenu> Menus { get; set; }
    }
}
