using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum SexEnum
    {
        秘密,
        男,
        女
    }
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypeEnum
    {
        管理员,
        用户
    }
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBTypeEnum
    {
        Mysql,
        SqlServer,
    }
    public enum PrivilegeTypeEnum
    {
        菜单=1,
        按钮=2
    }
}
