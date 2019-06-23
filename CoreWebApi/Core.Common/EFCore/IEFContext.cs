using Core.Model;
using Core.Model.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.EFCore
{
    /// <summary>
    /// 自定义EF接口
    /// </summary>
   public interface IEFContext
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        void AddEntity<T>(T entity) where T : BaseModel;

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        void UpdateEntity<T>(T entity) where T : BaseModel;

        /// <summary>
        /// 修改属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="field">属性</param>
        void UpdateProperty<T>(T entity, Expression<Func<T, object>> field) where T : BaseModel;

        /// <summary>
        /// 修改属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName">属性名</param>
        void UpdateProperty<T>(T entity, string fieldName) where T : BaseModel;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void DeleteEntity<T>(T entity) where T : BaseModel;


        /// <summary>
        /// 获取指定实体的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        //DbSet<T> Set<T>() where T : BaseModel;


        /// <summary>
        /// 级联删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void CascadeDelete<T>(T entity) where T : BaseModel, ITreeData<T>;


        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 执行sql语句或存储过程返回datatable
        /// </summary>
        /// <param name="sql">sql语句或存储过程名</param>
        /// <param name="commandType">类型</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        DataTable GetDataTableBySql(string sql, CommandType commandType, params object[] paras);

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// 数据库连接字符串Key
        /// </summary>
        string CSName { get; set; }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="allModules">所有模块</param>
        /// <returns>True:数据库初始化完成 False:即数据库初始化失败，数据库已存在</returns>
        Task<bool> DataInit(object allModules);

        IEFContext CreateNew();
        IEFContext ReCreate();
    }
}
