using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    public class SystemFunctionPrivilege:BaseModel
    {
        [Display(Name = "角色")]
        public Guid? RoleID { get; set; }
        [Display(Name = "用户")]
        public Guid? UserID { get; set; }
        [Display(Name ="动作")]
        public Guid? AcitonID { get; set; }
        public SystemAction Action { get; set; }
        [Display(Name = "是否允许")]
        public bool? Allowed { get; set; }
    }
}
