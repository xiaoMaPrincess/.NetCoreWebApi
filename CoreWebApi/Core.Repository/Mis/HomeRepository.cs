using Core.Common.EFCore;
using Core.IRepository.Mis;
using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Core.Repository.Mis
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IEFContext _context;
        public HomeRepository(IEFContext context)
        {
            _context = context;
        }

        public Task<List<SystemMenu>> GetMenuList(Guid UserID)
        {
            throw new NotImplementedException();
        }
        //public Task<List<SystemMenu>> GetMenuList(Guid UserID)
        //{
        //    if (UserID==null)
        //    {
        //        return null;
        //    }
        //    var roleList= _context.Set<SystemUserRole>().Where(x => x.UserID == UserID);
        //}
    }
}
