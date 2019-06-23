using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 角色
    /// </summary>
   public class SystemRole:BaseModel
    {
        [Display(Name ="角色编号")]
        [StringLength(100,ErrorMessage ="{0}最大100位")]
        [Required(ErrorMessage = "{0}是必填项")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0}必须是数字")]
        public string RoleCode { get; set; }

        [Display(Name = "角色名称")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string RoleName { get; set; }
        [Display(Name = "备注")]
        public string RoleRemark { get; set; }

    }
}
