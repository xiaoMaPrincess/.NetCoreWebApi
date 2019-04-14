using Core.Common.Dapper;
using Core.IRepository;
using Core.Model.Models;
using Core.Repository.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class BlogArticleRepository : BaseRepository<BlogArticle>, IBlogArticleRepository
    {
        private readonly DbContext db;
        public BlogArticleRepository(DbContext _db)
        {
            db = _db;
        }
        public BlogArticle GetBlog()
        {
            string sql = "SELECT * FROM BlogArticle";
            var list = DbContext.QueryFirstOrDefault<BlogArticle>(sql);

            return list; //new BlogArticle() { btitle="1" };// 
        }
    }
}
