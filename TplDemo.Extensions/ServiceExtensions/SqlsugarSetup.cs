using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TplDemo.Common.Cache;
using TplDemo.Common.Config;

namespace TplDemo.Extensions.ServiceExtensions
{
    public static class SqlsugarSetup
    {
        /// <summary></summary>
        /// <param name="services"></param>
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 缓存
            ICacheService myCache = new SugarCache();

            #region 数据库连接服务注入

            var configuration = BaseConfigModel.Configuration;
            // 连接字符串
            var listConfig = new List<IocConfig>();
            var databaseConfig = configuration.GetSection("database").Get<List<Dbconfig>>();
            if (databaseConfig.Count == 0)
            {
                throw new ArgumentNullException("请配置数据库连接");//
            }

            databaseConfig.Where(c => c.isclose == false).ToList().ForEach(m =>
            {
                listConfig.Add(new IocConfig()
                {
                    ConfigId = m.dbname,
                    ConnectionString = m.dburl,
                    DbType = (IocDbType)m.dbtype,
                    IsAutoCloseConnection = true,
                }
               );
            });
            services.AddSqlSugar(listConfig);
            // services.AddSqlsugarSetup();

            #endregion 数据库连接服务注入
        }

        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }

            return key;
        }
    }
}