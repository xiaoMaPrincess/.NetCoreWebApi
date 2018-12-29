using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Models
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class BlogArticle
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int bID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string bsubmitter { get; set; }
        /// <summary>
        /// 博客标题
        /// </summary>
        public string btitle { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string bcategory { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string bcontent { get; set; }
        /// <summary>
        /// 访问量
        /// </summary>
        public int btraffic { get; set; }
        /// <summary>
        /// 评论量
        /// </summary>
        public int bcommentNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime bUpdateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public DateTime bCreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bRemark { get; set; }
    }
}
