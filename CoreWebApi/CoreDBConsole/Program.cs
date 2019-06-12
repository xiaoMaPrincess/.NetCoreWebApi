using Core.Common.EFCore;
using Core.Common.Helper;
using Core.Model;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CoreDBConsole
{
    class Program
    {
      
        static void Main(string[] args)
        {

            MailMessage mailMsg = new MailMessage();//实例化对象
            mailMsg.From = new MailAddress("923974733@qq.com", "季某人");//源邮件地址和发件人
            mailMsg.To.Add(new MailAddress("xiaomaprincess@gmail.com"));//收件人地址
            mailMsg.Subject = "邮件发送测试";//发送邮件的标题
            StringBuilder sb = new StringBuilder();
            sb.Append("测试测试测试测试");
            sb.Append("嘿嘿");
            mailMsg.Body = sb.ToString();//发送邮件的内容
            //指定smtp服务地址（根据发件人邮箱指定对应SMTP服务器地址）
            SmtpClient client = new SmtpClient();//格式：smtp.126.com  smtp.164.com
            // 服务器名称
            client.Host = "smtp.qq.com";
            //端口
            client.Port = 587;
            //启用加密
            client.EnableSsl = true;
            //通过用户名和密码验证发件人身份
            client.Credentials = new NetworkCredential("923974733@qq.com", "xxxxxxxxxx");
            //发送邮件
            try
            {
                client.Send(mailMsg);
            }
            catch (SmtpException ex)
            {

            }
            Console.WriteLine("邮件已发送，请注意查收！");
            Console.ReadKey();

            //string connectionString = @"Server=localhost;Database=coreweb;User=root;Password=Princess;";
            //EFContext context = new EFContext(connectionString, DBTypeEnum.Mysql);
            //context.Database.EnsureCreated();
            //Console.WriteLine("数据库已初始化完毕，请查看！");

        }
    }
}
