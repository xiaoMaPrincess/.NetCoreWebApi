using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.Attributes;
using Core.IServices;
using Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Policy ="Admin")]
    public class BlogController : ControllerBase
    {
        private readonly IAdvertisementServices _advertisementServices;
        private readonly IBlogArticleServices _blogArticleServices;
        public BlogController(IAdvertisementServices advertisementServices, IBlogArticleServices blogArticleServices)
        {
            _advertisementServices = advertisementServices;
            _blogArticleServices = blogArticleServices;
        }

        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            return await _blogArticleServices.GetBlogs();
        }

        // GET: api/Blog
        /// <summary>
        /// Sum接口
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        [HttpGet]
        public int Get(int i,int j)
        {
            return _advertisementServices.Sum(i, j);
        }

        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Blog/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<List<Advertisement>> Get(int id)
        {
            return await _advertisementServices.Query(d => d.Id == id);
        }

        // POST: api/Blog
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
