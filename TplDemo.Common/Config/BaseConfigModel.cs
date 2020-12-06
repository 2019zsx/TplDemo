using Microsoft.Extensions.Configuration;
using System;

namespace TplDemo.Common.Config
{
    public class BaseConfigModel
    { /// <summary>
      ///
      /// </summary>
        public static IConfiguration Configuration { get; set; }

        public static JWTConfig jwtConfig { get; set; } = new JWTConfig();

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <param name="contentRootPath"></param>
        /// <param name="webRootPath"></param>
        public static void SetBaseConfig(IConfiguration config)
        {
            Configuration = config;
            try
            {
                jwtConfig.JWTSecretKey = Configuration["JwtAuth:SecurityKey"];
                jwtConfig.WebExp = double.Parse(Configuration["JwtAuth:WebExp"]);
                jwtConfig.WebRefresh = double.Parse(Configuration["JwtAuth:WebRefresh"]);
                jwtConfig.AppExp = double.Parse(Configuration["JwtAuth:AppExp"]);
                jwtConfig.AppRefresh = double.Parse(Configuration["JwtAuth:AppRefresh"]);
                jwtConfig.MiniProgramExp = double.Parse(Configuration["JwtAuth:MiniProgramExp"]);
                jwtConfig.MiniProgramRefresh = double.Parse(Configuration["JwtAuth:MiniProgramRefresh"]);
                jwtConfig.Issuer = Configuration["JwtAuth:Issuer"];
                jwtConfig.Audience = Configuration["JwtAuth:Audience"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}