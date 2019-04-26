using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 方法注释
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 返回Test实体
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<Test> GetModel()
        {
            return new Test { ID = 1, Name = "Jee", Email = "xiaomaprincess@gmail.com" };
        }
        /// <summary>
        /// 带参数的注释
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="name">账号</param>
        /// <returns></returns>
        [HttpPost]
        public string GetJwt(int ID,string name)
        {
            TokenModel tokenModel = new TokenModel
            {
                ID = ID,
                Name=name
            };

            return Token.GetJWT(tokenModel);
        }
    }
}
