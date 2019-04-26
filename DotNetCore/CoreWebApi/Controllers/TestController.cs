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
    /// 需认证的控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 认证通过之后可访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<TokenModel> Get([FromBody]TokenModel tokenModel)
        {
            return new TokenModel{ ID=1 };
        }

    }
}