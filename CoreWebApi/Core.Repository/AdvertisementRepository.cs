using Core.IRepository;
using Core.IRepository.BASE;
using Core.Model.Models;
using Core.Repository.BASE;
using Core.Repository.Sugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        //private DbContext context;
        //private SqlSugarClient db;
        //private SimpleClient<Advertisement> entityDB;

        //internal SqlSugarClient Db
        //{
        //    get { return db; }
        //    private set { db = value; }
        //}
        //public DbContext Context
        //{
        //    get { return context; }
        //    set { context = value; }
        //}

        ///// <summary>
        ///// 构造函数获取Sqlsugar实例
        ///// </summary>
        //public AdvertisementRepository()
        //{
        //    DbContext.Init(BaseDBConfig.ConnectionString);
        //    context = DbContext.GetDbContext();
        //    db = context.Db;
        //    entityDB = context.GetEntityDB<Advertisement>(db);
        //}

        //public int Add(Advertisement model)
        //{
        //    var i= db.Insertable(model).ExecuteReturnIdentity();
        //    return i;
        //}

        //public bool Delete(Advertisement model)
        //{
        //    var i= db.Deleteable(model).ExecuteCommand();
        //    return i > 0;
        //}

        //public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        //{
        //    return entityDB.GetList(whereExpression);
        //}

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

        //public bool Update(Advertisement model)
        //{
        //    //以主键为条件
        //    var i=  db.Updateable(model).ExecuteCommand();
        //    return i > 0;
        //}
       
    }
}
