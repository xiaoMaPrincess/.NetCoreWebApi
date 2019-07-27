using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.SearchModels
{
    public class UserSearch : SearchBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string ITCode { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
    }
}
