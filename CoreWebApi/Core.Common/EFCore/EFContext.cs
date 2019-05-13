using Core.Common.Helper;
using Core.Model;
using Core.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Common.EFCore
{
    public class EFContext: DbContext
    {
        /// <summary>
        /// 用户
        /// </summary>

        public DbSet<SystemUser> SystemUser { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 数据库类型 默认sqlserver
        /// </summary>
        public DBTypeEnum DbType { get; set; }
        public EFContext()
        {
            Connection = "Server=localhost;Database=mysqlcore;User=root;Password=Princess;";
        }
        public EFContext(string cs)
        {
            Connection = cs;
        }
        public EFContext(string cs,DBTypeEnum dbType)
        {
            Connection = cs;
            DbType = dbType;

        }

        /// <summary>
        /// 将实体状态设置为添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void AddEntity<T>(T entity) where T : class
        {
            // 更改指定实体的状态
            this.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// 将实体状态设置为更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void UpdateEntity<T>(T entity) where T : class
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 将一个实体的某个字段设为修改状态，用于只更新个别字段的情况
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="field">要修改的字段</param>
        public void UpdateProperty<T>(T entity, Expression<Func<T, object>> field) where T : class
        {
            var set = this.Set<T>();
            //if (set.Local.Where(x=>x))
            //{

            //}
            this.Entry(entity).Property(field).IsModified = true;
        }

        /// <summary>
        /// 同上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        public void UpdateProperty<T>(T entity, string fieldName) where T : class
        {
            var set = this.Set<T>();
            this.Entry(entity).Property(fieldName).IsModified = true;
        }

        /// <summary>
        /// 将一个实体的状体设置为删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteEntity<T>(T entity) where T : class
        {

            this.Entry(entity).State = EntityState.Modified;

            //var set = this.Set<T>();
            ////    set.Attach(entity);
            //set.Remove(entity);
        }

        /// <summary>
        /// 重写OnConfiguring 配置要连接的数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 根据类型使用对应的数据库。目前只支持SqlServer和MySql
            switch (DbType)
            {
                case DBTypeEnum.SqlServer:
                    optionsBuilder.UseSqlServer(Connection);
                    break;
                case DBTypeEnum.Mysql:
                    optionsBuilder.UseMySql(Connection);
                    break;
            }
        }
    }
}
