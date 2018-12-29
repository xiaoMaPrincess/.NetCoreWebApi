using Core.IServices.BASE;
using Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IBlogArticleServices:IBaseServices<BlogArticle>
    {
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns></returns>
        Task<List<BlogArticle>> GetBlogs();
    }
}
