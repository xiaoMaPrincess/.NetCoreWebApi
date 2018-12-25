using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    /// <summary>
    /// Todo控制器
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles ="Admin,Client")]
    [Authorize(Policy ="SystemOrAdmin")]
    public class TodoItemController : ControllerBase
    {
        /// <summary>
        /// 获取待办事项列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
        public List<TodoItem> GetAll()
        {
            var ss = new List<TodoItem>();
            return ss;
        }
        /// <summary>
        /// 根据ID获取特定数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTodo")]
        [ProducesResponseType(200)]
        public a GetById(long id)
        {
            var result = new a();
            return result;
        }
        /// <summary>
        /// FromBody 特性从情报体中获取参数，2.1版本之后不用添加
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]// 数据注释，告诉客户端返回类型
        public IActionResult Create([FromBody]TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            // 添加成功后跳转到名为GetById的route
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }
        /// <summary>
        /// Put请求：更新数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }
            return NoContent();
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return NoContent();
        }
    }
}
