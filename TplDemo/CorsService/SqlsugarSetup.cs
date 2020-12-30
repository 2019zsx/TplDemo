using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Common;
using TplDemo.Common.Cache;
using TplDemo.Common.Config;
using TplDemo.Common.Helper;

namespace TplDemo.CorsService
{
    /// <summary>SqlSugar 启动服务</summary>
    public static class SqlsugarSetup
    {
        /// <summary></summary>
        /// <param name="services"></param>
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            ICacheService myCache = new SugarCache();// 缓存
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
                          ConfigureExternalServices = new ConfigureExternalServices()
                          {
                              DataInfoCacheService = myCache //配置我们创建的缓存类
                          },

                          //db.Aop.OnDiffLogEvent = it =>
                          //{
                          //    var editBeforeData = it.BeforeData;//操作前记录  包含： 字段描述 列名 值 表名 表描述
                          //    var editAfterData = it.AfterData;//操作后记录   包含： 字段描述 列名 值  表名 表描述
                          //    var sql = it.Sql;
                          //    var parameter = it.Parameters;
                          //    var data = it.BusinessData;//这边会显示你传进来的对象
                          //    var time = it.Time;
                          //    var diffType = it.DiffType;//enum insert 、update and delete

                          //    //Write logic
                          //};
                          AopEvents = new AopEvents
                          {
                              OnDiffLogEvent = it =>
                              {
                                  //var editBeforeData = it.BeforeData;//操作前记录  包含： 字段描述 列名 值 表名 表描述
                                  //var editAfterData = it.AfterData;//操作后记录   包含： 字段描述 列名 值  表名 表描述
                                  //var sql = it.Sql;
                                  //var parameter = it.Parameters;
                                  //var data = it.BusinessData;//这边会显示你传进来的对象
                                  //var time = it.Time;
                                  //var diffType = it.DiffType;//enum insert 、update and delete

                                  //Write logic
                              },
                              OnLogExecuting = (sql, p) =>
                                      {
                                          string Parameter = GetParas(p);
                                          // Log.Error($"{sql}---{Parameter}");
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