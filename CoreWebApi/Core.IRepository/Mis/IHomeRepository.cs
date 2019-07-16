using Core.Model;
using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.Mis
{
    public interface IHomeRepository: IDependencyMsi
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        Task<List<SystemMenu>> GetMenuList(Guid UserID);
    }
}
