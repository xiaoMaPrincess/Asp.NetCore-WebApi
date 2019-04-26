using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi
{
    /// <summary>
    /// 身份验证中间件
    /// </summary>
    public class TokenAuth
    {
        /// <summary>
        /// http委托
        /// </summary>
        public readonly RequestDelegate _next;

        public TokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (!headers.ContainsKey("Authorization"))
            {
                return _next(context);
            }
            var tokenStr = headers["Authorization"];
            try { 
            string jwtStr = tokenStr.ToString().Substring("Bearer ".Length).Trim();
            //TokenModel tm = ((TokenModel)RayPIMemoryCache.Get(jwtStr));
            //提取tokenModel中的Sub属性进行authorize认证
            //context.
            //List<Claim> lc = new List<Claim>();
            //Claim c = new Claim(tm.Sub + "Type", tm.Sub);
            //lc.Add(c);
            //ClaimsIdentity identity = new ClaimsIdentity(lc);
            //ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            //context.User = principal;
            return _next(context);
            }
            catch (Exception)
            {
                return context.Response.WriteAsync("验证异常");
            }
        }
    }
}
