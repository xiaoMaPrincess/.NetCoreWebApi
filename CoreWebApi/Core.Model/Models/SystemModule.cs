using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 模块
    /// </summary>
   public class SystemModule:BaseModel
    {
        [Display(Name = "模块名称")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string ModuleName { get; set; }

        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [Display(Name = "类名")]
        public string ClassName { get; set; }

        [Display(Name = "动作")]
        public List<SystemAction> Actions { get; set; }
        //[Display(Name = "区域")]
        //public Guid? AreaId { get; set; }
        [Display(Name ="命名空间")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        public string NameSpace { get; set; }
        [NotMapped]
        public bool IsApi { get; set; }

    }
}
