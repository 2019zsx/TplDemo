using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TplDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging((context, logBuilder) =>
             {
                 // 1.���˵�ϵͳĬ�ϵ�һЩ��־
                 logBuilder.AddFilter("System", LogLevel.Error);
                 logBuilder.AddFilter("Microsoft", LogLevel.Error);
                 // 3.ͳһ����
                 logBuilder.SetMinimumLevel(LogLevel.Error);
                 var path = System.IO.Directory.GetCurrentDirectory();
                 logBuilder.AddLog4Net($"{path}/log4net.config");//�����ļ�
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}