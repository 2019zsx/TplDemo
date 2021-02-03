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
            #region ��ȡ�����ļ�

            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var applicationExeDirectory = Path.GetDirectoryName(location);
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot _Configuration = builder.Build();

            BaseConfigModel.SetBaseConfig(_Configuration);
            Configuration = configuration;

            #endregion ��ȡ�����ļ�
        }

        public IConfiguration Configuration { get; }

        /// <summary></summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region Session�ڴ滺��

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            ////�����ڴ滺��(�ò�������AddSession()����ǰʹ��)
            //services.AddDistributedMemoryCache();//����session֮ǰ����������ڴ�
            ////services.AddSession();
            //services.AddSession(options =>
            //{
            //    options.Cookie.Name = ".AgencyCmsWebapi.Session";
            //    options.IdleTimeout = TimeSpan.FromSeconds(20000);//����session�Ĺ���ʱ��
            //    options.Cookie.HttpOnly = true;//���������������ͨ��js��ø�cookie��ֵ
            //});

            #endregion Session�ڴ滺��

            #region ע�������

            services
           .AddControllers(options =>
           {
               options.Filters.Add(typeof(CustomResultFilter));
               options.Filters.Add(typeof(CustomExceptionFilterAttribute));
           })
            .AddJsonOptions(options =>
            {
                // options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                //ʱ���ʽת��
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                // ��� ǰ������string���ʹ���̨int ���ͽ��ձ�400����
                options.JsonSerializerOptions.Converters.Add(new IntToStringConverter());
                // �շ��ʽ
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;//
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            #endregion ע�������

            #region ����Swagger�ĵ�

            services.AddSwaggerService();

            #endregion ����Swagger�ĵ�

            #region ע����Ȩ��֤

            services.Configure<JWTConfig>(Configuration.GetSection("JwtAuth"));//��ȡ�����ļ�

            var jwtConfig = Configuration.GetSection("JwtAuth").Get<JWTConfig>();
            services.AddRayAuthService(jwtConfig);

            #endregion ע����Ȩ��֤

            #region ע��Cors����

            //ע��Cors����
            services.AddCorsService();

            #endregion ע��Cors����

            #region ���ݿ����ӷ���ע��

            services.AddSqlsugarSetup();

            #endregion ���ݿ����ӷ���ע��

            #region AutoMapper ��Ӧ����ת��

            services.AddAutoMapper(typeof(MapperProfiles).Assembly);

            #endregion AutoMapper ��Ӧ����ת��

            #region ע��http�����ķ�����

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            #endregion ע��http�����ķ�����

            #region ����û���Ϣע��

            services.AddScoped<IUser, User>();

            #endregion ����û���Ϣע��

            #region ֧��ע��

            services.AddAlipay();
            services.AddWeChatPay();

            // �� appsettings.json(����������appsettings.Development.json) �� ����ѡ��
            services.Configure<AlipayOptions>(Configuration.GetSection("Alipay"));
            services.Configure<WeChatPayOptions>(Configuration.GetSection("WeChatPay"));

            #endregion ֧��ע��

            #region ��Ȩ

            services.AddAuthorizationService();

            #endregion ��Ȩ
        }

        /// <summary>ע����Program.CreateHostBuilder�����Autofac���񹤳�</summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //�������ע���ϵ
            builder.RegisterModule(new AutofacModuleRegister());
            var controllerBaseType = typeof(ControllerBase);
            //�ڿ�������ʹ������ע��
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

            #region �����־

            // log4net.LogManager.GetLogger("");

            // logger.CreateLogger("");
            logger.AddLog4Net();

            #endregion �����־

            #region �����������������⣨nuget =��System.Text.Encoding.CodePages��

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            #endregion �����������������⣨nuget =��System.Text.Encoding.CodePages��

            app.UseRouting();
            //�������
            app.UseCors("Limit");
            // ����ĵ�˵��
            app.UseSwaggerService();
            //���������ʾ����
            app.UseErrorHandling();
            // ������֤Ȩ��
            app.UseAuthService();
            //UseSession������UseMvc֮ǰ
            //app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger("{documentName}/api-docs");
            });
        }
    }
}