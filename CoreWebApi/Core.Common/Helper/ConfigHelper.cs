using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Helper
{
    /// <summary>
    /// 配置信息映射实体
    /// </summary>
    public class AppSettings
    {
        public RedisCaching RedisCaching { get; set; }
        public SqlServer SqlServer { get; set; }
        public Mysql Mysql { get; set; }

    }
    public class RedisCaching
    {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }
    }
    public class SqlServer
    {
        public string Value { get; set; }
    }
    public class Mysql
    {
        public string Value { get; set; }
    }
}
