using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TplDemo.Common;
using TplDemo.Common.Config;
using TplDemo.Common.HttpContextUser;
using TplDemo.CorsService;
using TplDemo.Extensions;
using TplDemo.Extensions.AutofacModule;
using AutoMapper;
using TplDemo.Extensions.Mapper;
using TplDemo.Common.Helper;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.Alipay;
using TplDemo.Common.Filter;

namespace TplDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            #region 读取配置文件

            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var applicationExeDirectory = Path.GetDirectoryName(location);
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot _Configuration = builder.Build();

            BaseConfigModel.SetBaseConfig(_Configuration);
            Configuration = configuration;

            #endregion 读取配置文件
        }

        public IConfiguration Configuration { get; }

        /// <summary></summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region Session内存缓存

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            ////启用内存缓存(该步骤需在AddSession()调用前使用)
            //services.AddDistributedMemoryCache();//启用session之前必须先添加内存
            ////services.AddSession();
            //services.AddSession(options =>
            //{
            //    options.Cookie.Name = ".AgencyCmsWebapi.Session";
            //    options.IdleTimeout = TimeSpan.FromSeconds(20000);//设置session的过期时间
            //    options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该cookie的值
            //});

            #endregion Session内存缓存

            #region 注册控制器

            services
           .AddControllers(options =>
           {
               options.Filters.Add(typeof(CustomResultFilter));
               options.Filters.Add(typeof(CustomExceptionFilterAttribute));
           })
            .AddJsonOptions(options =>
            {
                // options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                //时间格式转换
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                // 解决 前端数字string类型传后台int 类型接收报400错误
                options.JsonSerializerOptions.Converters.Add(new IntToStringConverter());
                // 驼峰格式
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;//
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            #endregion 注册控制器

            #region 配置Swagger文档

            services.AddSwaggerService();

            #endregion 配置Swagger文档

            #region 注册授权认证

            services.Configure<JWTConfig>(Configuration.GetSection("JwtAuth"));//获取配置文件

            var jwtConfig = Configuration.GetSection("JwtAuth").Get<JWTConfig>();
            services.AddRayAuthService(jwtConfig);

            #endregion 注册授权认证

            #region 注册Cors跨域

            //注册Cors跨域
            services.AddCorsService();

            #endregion 注册Cors跨域

            #region 数据库连接服务注入

            services.AddSqlsugarSetup();

            #endregion 数据库连接服务注入

            #region AutoMapper 对应类型转换

            services.AddAutoMapper(typeof(MapperProfiles).Assembly);

            #endregion AutoMapper 对应类型转换

            #region 注册http上下文访问器

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            #endregion 注册http上下文访问器

            #region 填充用户信息注入

            services.AddScoped<IUser, User>();

            #endregion 填充用户信息注入

            #region 支付注入

            services.AddAlipay();
            services.AddWeChatPay();

            // 在 appsettings.json(开发环境：appsettings.Development.json) 中 配置选项
            services.Configure<AlipayOptions>(Configuration.GetSection("Alipay"));
            services.Configure<WeChatPayOptions>(Configuration.GetSection("WeChatPay"));

            #endregion 支付注入

            #region 授权

            services.AddAuthorizationService();

            #endregion 授权
        }

        /// <summary>注意在Program.CreateHostBuilder，添加Autofac服务工厂</summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //添加依赖注入关系
            builder.RegisterModule(new AutofacModuleRegister());
            var controllerBaseType = typeof(ControllerBase);
            //在控制器中使用依赖注入
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            .PropertiesAutowired();
        }

        /// <summary></summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 添加日志

            // log4net.LogManager.GetLogger("");

            // logger.CreateLogger("");
            logger.AddLog4Net();

            #endregion 添加日志

            #region 解决输出中文乱码问题（nuget =》System.Text.Encoding.CodePages）

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            #endregion 解决输出中文乱码问题（nuget =》System.Text.Encoding.CodePages）

            app.UseRouting();
            //跨域添加
            app.UseCors("Limit");
            // 添加文档说明
            app.UseSwaggerService();
            //请求错误提示配置
            app.UseErrorHandling();
            // 配置认证权限
            app.UseAuthService();
            //UseSession配置在UseMvc之前
            //app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("{documentName}/api-docs");
            });
        }
    }
}