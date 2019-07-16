using Core.Common.EFCore;
using Core.IRepository.Mis;
using Core.Model.Models;
using Core.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Core.Model;

namespace Core.Repository.Mis
{
    public class SystemUserRepository : ISystemUserRepository
    {
        private readonly IEFContext _context;
        public SystemUserRepository(IEFContext context)
        {
            _context = context;
        }
        public async Task<PageModel<UserInfoVM>> GetUserList(string userName, string itCode, bool? IsValid, int pageIndex = 1, int pageSize = 10)
        {

            var query = from a in _context.Set<SystemUser>()
                        select new UserInfoVM
                        {
                            ID = a.ID,
                            ITCode = a.ITCode,
                            Name = a.Name,
                            Email = a.Email,
                            Sex = a.Sex,
                            IsValid = a.IsValid,
                            CreateTime=a.CreateTime
                        };
            var userRole = from a in _context.Set<SystemUserRole>()
                           join b in _context.Set<SystemRole>()
                           on a.RoleID equals b.ID
                           select new
                           {
                               ID = a.UserID,
                               RoleName = b.RoleName
                           };

            await query.ForEachAsync(x =>
            {
                x.RoleName = userRole.Where(a => a.ID == x.ID).Select(b => b.RoleName).ToList();
            });
            if (!string.IsNullOrEmpty(itCode))
            {
                query= query.Where(x => x.ITCode.Contains(itCode));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(x => x.ITCode.Contains(userName));
            }
            if (IsValid != null)
            {
                query = query.Where(x => x.IsValid==IsValid);
            }
            var list = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            PageModel<UserInfoVM> data = new PageModel<UserInfoVM>()
            {
                DataList = list,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count
            };
            return data;
        }
    }
}
