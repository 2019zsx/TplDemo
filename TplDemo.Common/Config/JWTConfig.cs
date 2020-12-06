using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Config
{
    /// <summary>
    ///
    /// </summary>
    public class JWTConfig
    {
        /// <summary>
        /// 密匙
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        ///
        /// </summary>

        public string Audience { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string JWTSecretKey = "This is JWT Secret Key";

        /// <summary>
        /// web端
        /// </summary>
        public double WebExp = 12;

        public double WebRefresh = 24;

        /// <summary>
        ///App端
        /// </summary>
        public double AppExp = 12;

        public double AppRefresh = 12;

        /// <summary>
        ///小程序端
        /// </summary>
        public double MiniProgramExp = 12;

        public double MiniProgramRefresh = 12;

        /// <summary>
        ///
        /// </summary>
        public double OtherExp = 12;

        public double OtherRefresh = 12;
    }
}