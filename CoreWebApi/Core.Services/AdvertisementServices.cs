using Core.IRepository;
using Core.IServices;
using Core.Model.Models;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Services
{
    public class AdvertisementServices : IAdvertisementServices
    {
        IAdvertisementRepository dal = new AdvertisementRepository();

        public int Add(Advertisement model)
        {
           return dal.Add(model);
        }

        public bool Delete(Advertisement model)
        {
            return dal.Delete(model);
        }

        public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        {
            return dal.Query(whereExpression);
        }

        /// <summary>
        /// 求和方法实现
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int Sum(int i, int j)
        {
            return dal.Sum(i, j);
        }

        public bool Update(Advertisement model)
        {
            return dal.Update(model);
        }
    }
}
