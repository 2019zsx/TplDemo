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
                 // 1.过滤掉系统默认的一些日志
                 logBuilder.AddFilter("System", LogLevel.Error);
                 logBuilder.AddFilter("Microsoft", LogLevel.Error);
                 // 3.统一设置
                 logBuilder.SetMinimumLevel(LogLevel.Error);
                 var path = System.IO.Directory.GetCurrentDirectory();
                 logBuilder.AddLog4Net($"{path}/log4net.config");//配置文件
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}