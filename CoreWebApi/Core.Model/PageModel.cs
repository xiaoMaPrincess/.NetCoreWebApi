using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// 分页通用类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageModel<T> where T : class
    {
        /// <summary>
        /// 页面索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页面条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (PageSize > 0)
                    return Count % PageSize == 0 ? Count / PageSize : (Count / PageSize) + 1;
                else
                    return Count;
            }

        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> Data { get; set; }
    }
}
