using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.SearchModels
{
    public class SearchBase
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page
        {
            get; set;
        }
        /// <summary>
        /// 条数
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
