using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 用户组（ID）
    /// </summary>
    public class SystemUserGroup:BaseModel
    {
        public SystemUser User { get; set; }
        [Display(Name ="用户")]
        public Guid UserID { get; set; }

        public SystemGroup Group { get; set; }
        [Display(Name ="用户组")]
        public Guid GroupID { get; set; }
    }
}
