using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// Model层的基类，所有需要映射成表的Model都可以继承这个类。
    /// 包括了一个为Guid的主键，以及创建时间等属性。
    /// 有特殊需求的可不继承此类，创建合适的自定义类即可。
    /// </summary>
    public class BaseModel
    {
        private Guid _guid;
        /// <summary>
        /// 主键ID
        /// </summary>
        [Display(Name ="ID")]
        [Key]
        public Guid ID
        {
            get
            {
                if (_guid == Guid.Empty)
                {
                    _guid = Guid.NewGuid();
                }
                return _guid;
            }
            set
            {
                _guid = value;
            }
           
        }

        [Display(Name ="创建时间")]
        public DateTime? CreateTime { get; set; }
        [Display(Name ="创建人")]
        [StringLength(50)]
        public string Creator { get; set; }
        [Display(Name ="修改时间")]
        public DateTime? UpdateTime { get; set; }
        [Display(Name ="修改人")]
        [StringLength(50)]
        public DateTime? Updater { get; set; }

    }
}
