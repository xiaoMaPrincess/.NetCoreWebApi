using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Core.Common.Helper
{
    public class MailHelper
    {

        public static void SendMail(string receiver, MailContent mailContent, MailConfig mailConfig)
        {
            MailMessage mailMsg = new MailMessage();//实例化对象
            mailMsg.From = new MailAddress(mailConfig.Sender, mailConfig.SenderName);//源邮件地址和发件人
            mailMsg.To.Add(new MailAddress(receiver));//收件人地址
            mailMsg.Subject = mailContent.Subject;//发送邮件的标题
            mailMsg.Body = mailContent.Body;//发送邮件的内容
            //指定smtp服务地址（根据发件人邮箱指定对应SMTP服务器地址）
            SmtpClient client = new SmtpClient();//格式：smtp.126.com  smtp.164.com
            // 服务器名称
            client.Host = mailConfig.Host;
            //端口
            client.Port = 587;
            //启用加密
            client.EnableSsl = true;
            //通过用户名和密码验证发件人身份  //
            client.Credentials = new NetworkCredential(mailConfig.Sender, mailConfig.Password);
            //发送邮件
            client.Send(mailMsg);
        }
    }

    /// <summary>
    /// 邮件内容
    /// </summary>
    public class MailContent
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
    }


    /// <summary>
    /// 邮箱配置
    /// </summary>
    public class MailConfig
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 发送人名称
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 授权码/密码
        /// </summary>
        public string Password { get; set; }
    
    }
}
