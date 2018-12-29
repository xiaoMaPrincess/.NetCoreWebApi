using Core.IRepository;
using Core.IServices;
using Core.Model.Models;
using Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        private readonly IBlogArticleRepository dal;

        public BlogArticleServices(IBlogArticleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
        public async Task<List<BlogArticle>> GetBlogs()
        {
            var blogList = await dal.Query(a=>a.bID>0,a=>a.bID);
            return blogList;
        }
    }
}
