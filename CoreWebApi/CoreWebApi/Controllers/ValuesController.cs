using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebApi.Controllers
{
    /// <summary>
    /// value控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Policy ="Admin")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(201)]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType(201)]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(201)]
        public void Delete(int id)
        {
        }
    }
}
