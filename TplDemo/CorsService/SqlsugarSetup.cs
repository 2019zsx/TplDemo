using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Common;
using TplDemo.Common.Config;

namespace TplDemo.CorsService
{
    /// <summary>SqlSugar 启动服务</summary>
    public static class SqlsugarSetup
    {
        /// <summary></summary>
        /// <param name="services"></param>
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 把多个连接对象注入服务，这里必须采用Scope，因为有事务操作
            services.AddScoped<ISqlSugarClient>(o =>
            {
                var configuration = BaseConfigModel.Configuration;
                // 连接字符串
                var listConfig = new List<ConnectionConfig>();
                var databaseConfig = configuration.GetSection("database").Get<List<Dbconfig>>();
                if (databaseConfig.Count == 0)
                {
                    throw new ArgumentNullException("请配置数据库连接");//
                }

                databaseConfig.Where(c => c.isclose == false).ToList().ForEach(m =>
                    {
                        listConfig.Add(new ConnectionConfig()
                        {
                            ConfigId = m.dbname,
                            ConnectionString = m.dburl,
                            DbType = (DbType)m.dbtype,
                            IsAutoCloseConnection = true,
                            IsShardSameThread = false,
                            AopEvents = new AopEvents
                            {
                                OnLogExecuting = (sql, p) =>
                                {
                                    // SQL语句日志
                                }
                            },
                            MoreSettings = new ConnMoreSettings()
                            {
                                IsAutoRemoveDataCache = true
                            }
                        }
                       );
                    });
                return new SqlSugarClient(listConfig);
            });
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