using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Common;
using TplDemo.Common.Config;

namespace TplDemo.CorsService
{
    /// <summary></summary>
    public static class AuthStartupExtension
    {
        public static IServiceCollection AddRayAuthService(this IServiceCollection services, JWTConfig jwtConfig)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     // AudienceValidator = (m, n, z) => { //这里采用动态验证的方式，在重新登陆时，刷新token，旧token就强制失效了
                     // return m != null && m.FirstOrDefault().Equals(jwtConfig.Audience); },
                     ValidAudience = jwtConfig.Audience,
                     ValidateAudience = true,
                     ValidIssuer = jwtConfig.Issuer,
                     ValidateIssuer = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero
                 };
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");//token过期
                         }
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = context =>
                     {
                         //验证不通过
                         //var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
                         //var claims = context.Principal.Claims;
                         //var _value = claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                         //userContext.Id = _value.ObjToInt();
                         return Task.CompletedTask;
                     }
                 };
             });
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            return services;
        }

        /// <summary></summary>
        /// <param name="app"></param>
        public static void UseAuthService(this IApplicationBuilder app)
        {
            //认证中间件
            app.UseAuthentication();

            //授权中间件
            app.UseAuthorization();
        }
    }
}