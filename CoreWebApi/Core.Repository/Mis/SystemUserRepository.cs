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
using Core.Model.SearchModels;

namespace Core.Repository.Mis
{
    public class SystemUserRepository : ISystemUserRepository
    {
        private readonly IEFContext _context;
        public SystemUserRepository(IEFContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="search">搜索字段</param>
        /// <returns></returns>
        public async Task<TableModel<UserInfoVM>> GetUserList(UserSearch search)
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
                            CreateTime = a.CreateTime
                        };
            var userRole = from a in _context.Set<SystemUserRole>()
                           join b in _context.Set<SystemRole>()
                           on a.RoleID equals b.ID
                           select new
                           {
                               ID = a.UserID,
                               RoleName = b.RoleName
                           };

            if (!string.IsNullOrEmpty(search.ITCode))
            {
                query = query.Where(x => x.ITCode.Contains(search.ITCode));// search.ITCode.Contains(x.ITCode));
            }
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.ITCode.Contains(search.ITCode));
            }
            await query.ForEachAsync(x =>
            {
                if (userRole.Where(a => a.ID == x.ID).FirstOrDefault() != null)
                {
                    x.RoleName = userRole.Where(a => a.ID == x.ID).Select(a => a.RoleName).ToList();
                }
            });
            var list = await query.Skip((search.Page-1) * search.
                Limit).Take(search.Limit).ToListAsync();
            TableModel<UserInfoVM> data = new TableModel<UserInfoVM>()
            {
                Data = list,
                Count = query.Count(),
                Code=0
            };
            return data;
        }
    }
}
