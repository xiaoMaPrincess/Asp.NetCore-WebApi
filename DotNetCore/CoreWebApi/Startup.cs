using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册Swagger服务
            services.AddSwaggerGen(c =>
            {
                // 添加文档信息
                c.SwaggerDoc("v1", new Info
                {
                    Title = "CoreWebApi",
                    Version = "v1",
                    Description = "ASP.NET CORE WebApi",
                    Contact = new Contact
                    {
                        Name = "Jee",
                        Email = "xiaomaprincess@gmail.com",
                        Url = "https://www.cnblogs.com/jixiaosa/"
                    }
                });

                #region 读取xml信息

                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                c.IncludeXmlComments(xmlPath, true);
                #endregion

                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，CoreAPI。
                var security = new Dictionary<string, IEnumerable<string>> { { "CoreAPI", new string[] { } }, };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("CoreAPI", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion

            });

            #region 添加验证服务

            // 添加验证服务
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes("xiaomaPrincess@gmail.com")),
                    ValidateIssuer = true,
                    ValidIssuer = "API",//发行人
                    ValidateAudience = true,
                    ValidAudience = "User",//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
            });
            #endregion

            // 基于策略的授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("system", policy => policy.RequireClaim("systemtype").Build());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // 启用认证中间件
            app.UseAuthentication();

            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreWebApi");
                c.RoutePrefix = string.Empty;
            });
            app.UseMiddleware<TokenAuth>();
            app.UseMvc();
        }
    }
}
