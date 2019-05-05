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
    /// 需要身份认证的控制器
    /// </summary>
    [Route("api/v2/[controller]/[action]")]
    [ApiController]
    [Authorize]// 添加授权特性
    [ApiVersion("2.0")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 认证通过之后可访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<TokenModel> Get(TokenModel tokenModel)
        {
            return new TokenModel{ ID=1 };
        }

    }
}