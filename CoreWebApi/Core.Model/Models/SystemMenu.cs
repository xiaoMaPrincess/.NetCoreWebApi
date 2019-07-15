using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Core.Model.Models
{
    /// <summary>
    /// 菜单（页面）
    /// </summary>
    public class SystemMenu:BaseModel
    {
        [Display(Name = "菜单名称")]
        [StringLength(50, ErrorMessage = "{0}最多输入{1}个字符")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string MenuName { get; set; }
        // 是否父节点
        [Display(Name ="目录")]
        [Required(ErrorMessage = "{0}是必填项")]
        public bool FolderOnly { get; set; }
        [Display(Name ="类名")]
        public string ClassName { get; set; }
        // 域 public Guid? DomainID { get; set; }
        [Display(Name = "菜单显示")]
        [Required(ErrorMessage = "{0}是必填项")]
        public bool ShowOnMenu { get; set; }

        [Display(Name = "公开")]
        [Required(ErrorMessage = "{0}是必填项")]
        public bool IsPublic { get; set; }
        [Display(Name ="显示顺序")]
        [Required(ErrorMessage = "{0}是必填项")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "内部地址")]
        [Required(ErrorMessage = "{0}是必填项")]
        public bool? IsInside { get; set; }

        [Display(Name ="地址")]
        public string Url { get; set; }

        // 图标关联文件表 public Guid? IConID { get; set; }
        [Display(Name ="父目录")]
        public Guid? ParentID { get; set; }
    }
}
