using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.ViewModels
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class LoginVM
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string ITCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 显示昵称
        /// </summary>
        public string Name { get; set; }
        ///// <summary>
        ///// 性别
        ///// </summary>
        //public SexEnum Sex { get; set; }
        /// <summary>
        /// 密码 加密
        /// </summary>
        public string PasswordMD5 { get; set; }
        
    }

    /// <summary>
    /// 用户注册（返回结果）
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// True：成功 False: 失败
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; set; }
    }
}
