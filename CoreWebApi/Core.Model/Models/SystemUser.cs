using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    public class SystemUser:BaseModel
    {
        [Display(Name = "账号")]
        [StringLength(50,ErrorMessage ="{0}最多输入{1}个字符")]
        [Required(ErrorMessage ="{0}是必填项")]
        public string ITCode { get; set; }

        [Display(Name = "密码")]
        [StringLength(32)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0}是必填项",AllowEmptyStrings =false)]
        public string Password { get; set; }

        [Display(Name="邮箱")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0}是必填项")]
        public string Email { get; set; }

        [Display(Name="用户名")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string Name { get; set; }

        [Display(Name="性别")]
        public SexEnum? Sex { get; set; }

        [Display(Name="手机")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression("^[1][3,4,5,7,8][0-9]{9}$", ErrorMessage = "{0}格式错误")]
        [StringLength(50)]
        public string CellPhone { get; set; }

        [Display(Name = "住址")]
        [StringLength(200, ErrorMessage = "{0}最多输入{1}个字符")]
        public string Address { get; set; }

        [Display(Name = "邮编")]
        //[RegularExpression("^[0-9]{6,6}$", ErrorMessage = "{0}必须是6位数字")]
        [DataType(DataType.PostalCode)]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        public string ZipCode { get; set; }
        /// <summary>
        /// 目前还不能使用
        /// </summary>
        [Display(Name="头像")]
        public Guid? PhotoId { get; set; }

        [Display(Name="是否有效")]
        public bool IsValid { get; set; }

        [Display(Name="用户类型")]
        public UserTypeEnum UserType { get; set; }
    }
}
