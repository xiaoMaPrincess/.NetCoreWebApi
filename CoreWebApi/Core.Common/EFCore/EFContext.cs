using Core.Common.Helper;
using Core.Model;
using Core.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.EFCore
{
    public class EFContext: DbContext,IEFContext
    {
        // 连接字符串key
        public string CSName { get; set; }

        /// <summary>
        /// 数据库类型 默认sqlserver
        /// </summary>
        public DBTypeEnum DbType { get; set; }

        public EFContext()
        {
            CSName = "server=127.0.0.1;uid=sa;pwd=sasa;database=core"; //"default";
        }

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="cs">数据库连接字符串</param>
        public EFContext(string cs)
        {
            CSName = cs;
        }

        /// <summary>
        /// 重载构造函数
        /// </summary>
        /// <param name="cs">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public EFContext(string cs, DBTypeEnum dbType)
        {
            CSName = cs;
            DbType = dbType;
        }

        public IEFContext CreateNew()
        {
            return (IEFContext)this.GetType().GetConstructor(new Type[] { typeof(string), typeof(IEFContext) }).Invoke(new object[] { CSName, DbType });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEFContext ReCreate()
        {
            return (IEFContext)this.GetType().GetConstructor(new Type[] { typeof(string), typeof(IEFContext) }).Invoke(new object[] { CSName, DbType }); ;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="allModules">所有模块</param>
        /// <returns>返回true即数据新建完成，进入初始化操作，返回false即数据库已经存在<</returns>
        public async virtual Task<bool> DataInit(object allModules)
        {
            // 创建数据库
            if (await Database.EnsureCreatedAsync())
            {
                var moduleList = allModules as List<SystemModule>;
                if (moduleList != null && moduleList.Count > 0)
                {
                    foreach (var module in moduleList)
                    {
                        module.CreateTime = DateTime.Now;
                        module.Creator = "system";
                        Set<SystemModule>().Add(module);
                    }
                }
                var role = new SystemRole() { RoleCode = "001", RoleName = "超级管理员", CreateTime = DateTime.Now, Creator = "system" };
                var user = new SystemUser()
                {
                    ITCode = "admin",
                    IsValid = true,
                    Name = "超级管理员",
                    Password =MD5Helper.GetMD5String("123456")
                };
                var userroles = new SystemUserRole()
                {
                    User = user,
                    Role = role
                };
                //var adminRole = role;
                // 判断是否存在菜单
                if (!this.Set<SystemMenu>().Any())
                {
                    // 初始化系统菜单
                    var sysManagement = GetFolderMenu("系统管理", role, null);
                    // 用户菜单
                    var userList = GetMenu(moduleList, "SystemUser", role, null, 1);
                    var roleList = GetMenu(moduleList, "SystemRole", role, null, 2);
                    var groupList = GetMenu(moduleList, "SystemGroup", role, null, 3);
                    var menuList = GetMenu(moduleList, "SystemMenu", role, null, 4);
                    sysManagement.ChildrenList.AddRange(new SystemMenu[] { userList, roleList, groupList, menuList });
                }
                Set<SystemUser>().Add(user);
                Set<SystemRole>().Add(role);
                Set<SystemUserRole>().Add(userroles);
                await SaveChangesAsync();
                return true;
            }
            return false;
        }

        //public DbSet<T> Set<T>() where T : class
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 将实体状态设置为添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void AddEntity<T>(T entity) where T : BaseModel
        {
            // 更改指定实体的状态
            this.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// 将实体状态设置为修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void UpdateEntity<T>(T entity) where T : BaseModel
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 将一个实体的某个字段设为修改状态，用于只更新个别字段的情况
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="field">要修改的字段</param>
        public void UpdateProperty<T>(T entity, Expression<Func<T, object>> field) where T : BaseModel
        {
            var set = this.Set<T>();
            if (set.Local.Where(x => x.ID == entity.ID).FirstOrDefault() == null)
            {
                set.Attach(entity);
            }
            this.Entry(entity).Property(field).IsModified = true;
        }

        /// <summary>
        /// 同上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        public void UpdateProperty<T>(T entity, string fieldName) where T : BaseModel
        {
            var set = this.Set<T>();
            if (set.Local.Where(x => x.ID == entity.ID).FirstOrDefault() == null)
            {
                set.Attach(entity);
            }
            this.Entry(entity).Property(fieldName).IsModified = true;
        }

        /// <summary>
        /// 将一个实体的状体设置为删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteEntity<T>(T entity) where T : BaseModel
        {
            var set = this.Set<T>();
            //this.Entry(entity).State = EntityState.Deleted;
            if (set.Local.Where(x => x.ID == entity.ID).FirstOrDefault() == null)
            {
                set.Attach(entity);
            }
            set.Remove(entity);
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
                    optionsBuilder.UseSqlServer(CSName);
                    break;
                case DBTypeEnum.Mysql:
                    optionsBuilder.UseMySql(CSName);
                    break;
                default:
                    break;
            }
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        /// <summary>
        /// 级联删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void CascadeDelete<T>(T entity) where T : BaseModel, ITreeData<T>
        {
            if (entity != null)
            {
                var set = this.Set<T>();
                var entityList = set.Where(x => x.ParentID == entity.ID).ToList();
                if (entityList.Count > 0)
                {

                    // 递归删除子节点
                    foreach (var item in entityList)
                    {
                        CascadeDelete(item);
                    }
                }
                DeleteEntity(entity);
            }
        }

        /// <summary>
        /// 执行Sql或存储过程返回datatable
        /// </summary>
        /// <param name="sql">sql语句或存储过程名</param>
        /// <param name="commandType">类型</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public DataTable GetDataTableBySql(string sql, CommandType commandType, params object[] paras)
        {
            DataTable table = new DataTable();
            switch (this.DbType)
            {
                case DBTypeEnum.SqlServer:
                    SqlConnection con = this.Database.GetDbConnection() as SqlConnection;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = commandType;
                        if (paras.Count() > 0)
                        {
                            cmd.Parameters.AddRange(paras);
                        }
                        adapter.SelectCommand = cmd;
                        adapter.Fill(table);
                        adapter.SelectCommand.Parameters.Clear();
                    }
                    break;
                case DBTypeEnum.Mysql:
                    MySqlConnection mySqlCon = this.Database.GetDbConnection() as MySqlConnection;
                    using (MySqlCommand cmd = new MySqlCommand(sql, mySqlCon))
                    {
                        if (mySqlCon.State == ConnectionState.Closed)
                        {
                            mySqlCon.Open();
                        }
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = commandType;
                        if (paras.Count() > 0)
                        {
                            cmd.Parameters.AddRange(paras);
                        }
                        MySqlDataReader dr = cmd.ExecuteReader();
                        table.Load(dr);
                        dr.Close();
                        mySqlCon.Close();
                    }
                    break;
            }
            return table;
        }


        /// <summary>
        /// 获取父级菜单
        /// </summary>
        /// <param name="FolderName">Name</param>
        /// <param name="roleList">可访问的角色</param>
        /// <param name="userList">可访问的用户</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="isInherite">是否可继承</param>
        /// <returns></returns>
        private SystemMenu GetFolderMenu(string FolderName, SystemRole role, SystemUser user, bool isShow = true, bool isInherite = false)
        {
            SystemMenu meun = new SystemMenu()
            {
                PageName = FolderName,
                ChildrenList = new List<SystemMenu>(),
                PrivilegeList = new List<SystemFunctionPrivilege>(),
                ShowOnMenu = isShow,
                IsInside = true,
                IsPublic = false,
                CreateTime = DateTime.Now,
                DisplayOrder = 1
            };
            if (role != null)
            {
                //  ???
                meun.PrivilegeList.Add(new SystemFunctionPrivilege { RoleID = role.ID, Allowed = true });
            }
            if (user != null)
            {
                meun.PrivilegeList.Add(new SystemFunctionPrivilege { UserID = user.ID, Allowed = true });
            }
            return meun;
        }

        /// <summary>
        /// 获取子级菜单
        /// </summary>
        /// <param name="moduleList">模块</param>
        /// <param name="controllerName">控制器</param>
        /// <param name="actionName">方法</param>
        /// <param name="role">角色</param>
        /// <param name="user">用户</param>
        /// <param name="displayOrder">显示顺序</param>
        /// <returns></returns>
        private SystemMenu GetMenu(List<SystemModule> moduleList, string controllerName, SystemRole role, SystemUser user, int displayOrder)
        {
            var actions = moduleList.Where(x => x.ClassName == controllerName && x.IsApi == true).SelectMany(x => x.Actions).ToList();
            var menu = GetMenuFromAction(actions[0], true, role, user, displayOrder);
            if (menu != null)
            {
                menu.Url = "/" + actions[0].Module.ClassName.ToLower();
                menu.ModuleName = actions[0].Module.ModuleName;
                menu.PageName = menu.ModuleName;
                menu.ActionName = "主页面";
                menu.ClassName = actions[0].Module.ClassName;
                menu.MethodName = null;
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i] != null)
                    {
                        // 获取子集
                        menu.ChildrenList.Add(GetMenuFromAction(actions[i], false, role, user, (i + 1)));
                    }
                }
            }
            return menu;
            ;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="act"></param>
        /// <param name="isMainLink"></param>
        /// <param name="role"></param>
        /// <param name="user"></param>
        /// <param name="displayOrder"></param>
        /// <returns></returns>
        private SystemMenu GetMenuFromAction(SystemAction act, bool isMainLink, SystemRole role, SystemUser user, int displayOrder = 1)
        {
            if (act == null)
            {
                return null;
            }
            SystemMenu menu = new SystemMenu
            {
                ClassName = act.Module.ClassName,
                MethodName = act.MethodName,
                Url = act.Url,
                PrivilegeList = new List<SystemFunctionPrivilege>(),
                ShowOnMenu = isMainLink,
                FolderOnly = false,
                ChildrenList = new List<SystemMenu>(),
                IsPublic = false,
                IsInside = true,
                DisplayOrder = displayOrder,
                CreateTime = DateTime.Now
            };
            if (isMainLink)
            {
                menu.ModuleName = act.Module.ModuleName;
                menu.PageName = act.Module.ModuleName;
                menu.ActionName = act.ActionName;
            }
            else
            {
                menu.PageName = act.ActionName;
                menu.ModuleName = act.Module.ModuleName;
                menu.ActionName = act.ActionName;
            }
            if (role != null)
            {
                menu.PrivilegeList.Add(new SystemFunctionPrivilege { RoleID = role.ID, Allowed = true });
            }
            if (user != null)
            {
                menu.PrivilegeList.Add(new SystemFunctionPrivilege { UserID = user.ID, Allowed = true });
            }
            return menu;

        }
    }
}
