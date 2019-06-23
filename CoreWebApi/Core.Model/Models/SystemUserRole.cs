using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 用户角色中间表
    /// </summary>
    public class SystemUserRole:BaseModel
    {
        [Display(Name ="用户")]
        public Guid UserID { get; set; }
        public SystemUser User { get; set; }
        [Display(Name ="角色")]
        public Guid RoleID { get; set; }
        public SystemRole Role { get; set; }
    }
}
