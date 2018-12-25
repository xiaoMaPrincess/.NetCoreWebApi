using Core.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        /// <summary>
        /// 求和方法实现
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public int Sum(int i, int j)
        {
            return i + j;
        }
    }
}
