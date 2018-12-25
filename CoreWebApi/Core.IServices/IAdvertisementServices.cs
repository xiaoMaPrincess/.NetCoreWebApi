using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.IServices
{
    public interface IAdvertisementServices
    {
        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        int Sum(int i, int j);
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(Advertisement model);
        /// <summary>
        /// 删
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Delete(Advertisement model);
        /// <summary>
        /// 改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(Advertisement model);
        /// <summary>
        /// 数据列表k
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
    }
}
