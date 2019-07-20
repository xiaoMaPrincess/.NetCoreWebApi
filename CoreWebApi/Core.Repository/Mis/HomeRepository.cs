using Core.Common.EFCore;
using Core.IRepository.Mis;
using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Core.Model.ViewModels;

namespace Core.Repository.Mis
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IEFContext _context;
        public HomeRepository(IEFContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 根据用户ID获取对应的菜单列表
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public async Task<List<MenuInfo>> GetMenuList(Guid userID)
        {
            if (userID == null)
            {
                return null;
            }
            var query = from a in _context.Set<SystemUserRole>()
                        join b in _context.Set<SystemPrivilege>()
                        on a.RoleID equals b.RoleID
                        join c in _context.Set<SystemMenu>()
                        on b.PrivilegeID equals c.ID
                        where a.UserID == userID && b.PrivilegeType == Model.PrivilegeTypeEnum.菜单 && c.ShowOnMenu == true
                        select new SystemMenu
                        {
                            ID = c.ID,
                            MenuName = c.MenuName,
                            Url = c.Url,
                            ParentID = c.ParentID,
                            DisplayOrder = c.DisplayOrder
                        };
            var folder = _context.Set<SystemMenu>().Where(x => x.FolderOnly == true && x.ShowOnMenu == true);
            List<MenuInfo> menuList = new List<MenuInfo>();
            if (folder.Count() > 0)
            {
            await Task.Run(() =>
            {
                foreach (var item in folder)
                {
                    var childList = query.Where(x => x.ParentID == item.ID);
                    if (childList.Count() > 0)
                    {
                        MenuInfo menu = new MenuInfo();
                        menu.ModuleName = item.MenuName;
                        menu.SystemMenus = childList.OrderBy(x => x.DisplayOrder).ToList();
                        menuList.Add(menu);
                    }
                }
            });
            }

            return menuList;

        }
    }
}
