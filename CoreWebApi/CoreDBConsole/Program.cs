using Core.Common.EFCore;
using Core.Common.Helper;
using Core.Model;
using Microsoft.Extensions.Options;
using System;

namespace CoreDBConsole
{
    class Program
    {
        /// <summary>
        ///  连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }
        public Program(IOptions<AppSettings> options)
        {
            ConnectionString = options.Value.Mysql.Value;
        }
        static void Main(string[] args)
        {
            EFContext context = new EFContext(ConnectionString, DBTypeEnum.Mysql);
            context.Database.EnsureCreated();
            Console.WriteLine("数据库已初始化完毕，请查看！");
            Console.ReadKey();
        }
    }
}
