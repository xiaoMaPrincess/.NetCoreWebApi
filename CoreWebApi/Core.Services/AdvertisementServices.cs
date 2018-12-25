using Core.IRepository;
using Core.IServices;
using Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class AdvertisementServices : IAdvertisementServices
    {
        IAdvertisementRepository dal = new AdvertisementRepository();
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
    }
}
