using CoreWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebApi
{
    /// <summary>
    /// 生成JWT字符串
    /// </summary>
    public class Token
    {
        /// <summary>
        /// 生成JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string GetJWT(TokenModel tokenModel)
        {
            //DateTime utc = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.ID.ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iat,$"{DateTime.UtcNow}"), // 令牌颁发时间
                new Claim(JwtRegisteredClaimNames.Nbf,$"{DateTime.UtcNow}"),
                new Claim(JwtRegisteredClaimNames.Exp,$"{DateTime.UtcNow.AddSeconds(100)}"), // 过期时间
                new Claim(JwtRegisteredClaimNames.Iss,"API"), // 签发者
                new Claim(JwtRegisteredClaimNames.Aud,"User") // 接收者
            };

            // 可以将一个用户的多个角色全部赋予；
            //claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));
            // 密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("xiaomaPrincess@gmail.com"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                //audience: tokenModel.Name,// 接收者
                claims: claims,// 声明的集合
                               //expires: .AddSeconds(36), // token的有效时间
                signingCredentials: creds
                );
            var handler = new JwtSecurityTokenHandler();
            // 生成 jwt字符串
            var strJWT = handler.WriteToken(jwt);
            return strJWT;
        }

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJwt(string jwtStr)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = handler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwt.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModel
            {
                ID = int.Parse(jwt.Id)
                //Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }


    }

}
