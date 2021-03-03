using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Extensions.ServiceExtensions
{
    public static class CorsDiExtension
    {
        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            #region Cors 跨域

            // 设置允许所有来源跨域
            services.AddCors(options => options.AddPolicy("Limit",
            builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true) // =AllowAnyOrigin()
                    .AllowCredentials();
            }));
            //services.AddCors(options =>
            //{ //options.AddPolicy("Limit", policy =>
            //    //{
            //    //    policy
            //    //    //.WithOrigins("http://localhost:9528", "http://localhost:9529", "http://localhost:9527", "http://106.15.38.78:81", "http://106.15.38.78:80", "http://106.15.38.78:3301")
            //    //    .AllowAnyHeader()
            //    //    .AllowAnyMethod()
            //    //    .AllowCredentials();
            //    //});

            // //options.AddPolicy("Limit", policy => //{ // policy //
            // //.WithOrigins("http://localhost:9528", "http://localhost:9529",
            // "http://localhost:9527", "http://106.15.38.78:81", "http://106.15.38.78:80",
            // "http://106.15.38.78:3301") // .AllowAnyHeader() // .AllowAnyMethod() //
            // .AllowCredentials(); //});

            //    /*
            //    //浏览器会发起2次请求,使用OPTIONS发起预检请求，第二次才是api异步请求
            //    options.AddPolicy("All", policy =>
            //    {
            //        policy
            //        .AllowAnyOrigin()
            //        .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials();
            //    });
            //    */
            //});

            #endregion Cors 跨域

            return services;
        }
    }
}