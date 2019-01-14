using AutoMapper;
using Core.IRepository;
using Core.IServices;
using Core.Model.Models;
using Core.Model.ViewModels;
using Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        private readonly IBlogArticleRepository dal;
        
        private readonly IMapper _mapper;

        public BlogArticleServices(IBlogArticleRepository dal,IMapper mapper)
        {
            this.dal = dal;
            base.baseDal = dal;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogViewModel> getBlogDetails(int id)
        {
            var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);
            var idmin = bloglist.FirstOrDefault() != null ? bloglist.FirstOrDefault().bID : 0;
            var idmax = bloglist.LastOrDefault() != null ? bloglist.LastOrDefault().bID : 1;
            var idminshow = id;
            var idmaxshow = id;

            BlogArticle blogArticle = new BlogArticle();

            blogArticle = (await dal.Query(a => a.bID == idminshow)).FirstOrDefault();

            BlogArticle prevblog = new BlogArticle();


            while (idminshow > idmin)
            {
                idminshow--;
                prevblog = (await dal.Query(a => a.bID == idminshow)).FirstOrDefault();
                if (prevblog != null)
                {
                    break;
                }
            }

            BlogArticle nextblog = new BlogArticle();
            while (idmaxshow < idmax)
            {
                idmaxshow++;
                nextblog = (await dal.Query(a => a.bID == idmaxshow)).FirstOrDefault();
                if (nextblog != null)
                {
                    break;
                }
            }


            blogArticle.btraffic += 1;
            await dal.Update(blogArticle, new List<string> { "btraffic" });

            BlogViewModel models = _mapper.Map<BlogViewModel>(blogArticle);
            //BlogViewModel models = new BlogViewModel()
            //{
            //    bsubmitter = blogArticle.bsubmitter,
            //    btitle = blogArticle.btitle,
            //    bcategory = blogArticle.bcategory,
            //    bcontent = blogArticle.bcontent,
            //    btraffic = blogArticle.btraffic,
            //    bcommentNum = blogArticle.bcommentNum,
            //    bUpdateTime = blogArticle.bUpdateTime,
            //    bCreateTime = blogArticle.bCreateTime,
            //    bRemark = blogArticle.bRemark,
            //};

            if (nextblog != null)
            {
                models.next = nextblog.btitle;
                models.nextID = nextblog.bID;
            }

            if (prevblog != null)
            {
                models.previous = prevblog.btitle;
                models.previousID = prevblog.bID;
            }
            return models;
        }

        [Common.Attributes.Caching(AbsoluteExpiration = 10)] //添加缓存特性
        public async Task<List<BlogArticle>> GetBlogs()
        {
            var blogList = await dal.Query(a=>a.bID>0,a=>a.bID);
            return blogList;
        }

        public BlogArticle GetBlog()
        {
           return dal.GetBlog();
        }
    }
}
