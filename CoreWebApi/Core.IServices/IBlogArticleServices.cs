using Core.IServices.BASE;
using Core.Model.Models;
using Core.Model.ViewModels;
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
        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BlogViewModel> getBlogDetails(int id);

        /// <summary>
        /// Dapper测试
        /// </summary>
        /// <returns></returns>
        BlogArticle GetBlog();
    }
}
