using Core.IRepository;
using Core.IServices;
using Core.Model.Models;
using Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        IAdvertisementRepository dal;
        // 构造函数注入
        public AdvertisementServices(IAdvertisementRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
        public int Sum(int i, int j)
        {
            return dal.Sum(i, j);
        }
    }
}
