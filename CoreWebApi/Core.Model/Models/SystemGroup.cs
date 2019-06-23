using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class SystemGroup:BaseModel
    {
        [Display(Name ="用户组编号")]
        [StringLength(100, ErrorMessage = "{0}最大100位")]
        [Required(ErrorMessage = "{0}是必填项")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0}必须是数字")]
        public string GroupCode { get; set; }
        [Display(Name = "用户组名称")]
        [StringLength(100, ErrorMessage = "{0}最多输入{1}个字符")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string GroupName { get; set; }
    }
}
