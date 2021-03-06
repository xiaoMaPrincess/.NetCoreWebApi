﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.Attributes;
using Core.Common.Dapper;
using Core.Common.Helper;
using Core.Common.Redis;
using Core.IServices;
using Core.Model.Models;
using Core.Model.ViewModels;
using CoreWebApi.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static CoreWebApi.SwaggerHelper.CustomApiVersion;

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
        private readonly IRedisCacheManager _redisCacheManager;
        private readonly IOptions<AppSettings> _options;
        private ILogger<BlogController> logger;
        public BlogController(IAdvertisementServices advertisementServices, IBlogArticleServices blogArticleServices,IRedisCacheManager redisCacheManager, IOptions<AppSettings> options, ILogger<BlogController> _logger)
        {
            _advertisementServices = advertisementServices;
            _blogArticleServices = blogArticleServices;
            _redisCacheManager = redisCacheManager;
            _options = options;
            logger = _logger;
        }

        /// <summary>
        /// 测试接口版本控制
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomRoute(ApiVersions.v2)]
        public object GetVAsync_v2()
        {
            return Ok(new { status = 220, data = "接口第二版" });
        }

        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            // 获取配置信息
            var con = _options.Value.RedisCaching.ConnectionString;
           // var connect = AppsettingsHelper.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });
            List<BlogArticle> blogArticles = new List<BlogArticle>();
            if (_redisCacheManager.Get<object>("Redis.Blog") != null)
            {
                blogArticles = _redisCacheManager.Get<List<BlogArticle>>("Redis.Blog");
            }
            else
            {
                blogArticles = await _blogArticleServices.GetBlogs();
                _redisCacheManager.Set("Redis.Blog", blogArticles, TimeSpan.FromHours(2));
            }
            return blogArticles;
        }

        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BlogViewModel> GetBlogDetails(int id)
        {
            return await _blogArticleServices.getBlogDetails(id);
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

        /// <summary>
        /// Dapper测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BlogArticle GetBlogArticle()
        {
            //_redisCacheManager.Set("jee", "haha");
            return _blogArticleServices.GetBlog();
        }
        /// <summary>
        /// Nlog日志接口测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> SetLog()
        {
            logger.LogWarning("Nlog ceshi");
            return new string[] { "value1", "value2" };
        }

    }
}
