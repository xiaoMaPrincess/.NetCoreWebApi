using Core.IRepository;
using Core.Model.Models;
using Core.Repository.BASE;
using Core.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class BlogArticleRepository : BaseRepository<BlogArticle>, IBlogArticleRepository
    {
        public BlogArticle GetBlog()
        {
            string sql = "SELECT * FROM BlogArticle";
            var list= DbContext.Query<BlogArticle>(sql);
            return list.FirstOrDefault();
        }
    }
}
