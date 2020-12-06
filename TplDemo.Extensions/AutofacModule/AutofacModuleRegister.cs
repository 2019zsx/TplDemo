using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TplDemo.Repository.BASE;

namespace TplDemo.Extensions.AutofacModule
{
    public class AutofacModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "TplDemo.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "TplDemo.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                // log.Error(msg);
                throw new Exception(msg);
            }

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();//注册仓储

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = System.Reflection.Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                   .InstancePerDependency();

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = System.Reflection.Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            #endregion 带有接口层的服务注入
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        public static System.Reflection.Assembly GetAssemblyByName(String AssemblyName)
        {
            return System.Reflection.Assembly.Load(AssemblyName);
        }
    }
}