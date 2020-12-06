using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Config
{
    public class RequestTypeModel
    {
        /// <summary>
        ///
        /// </summary>
        public int? Tokenexp { get; set; } = 0;

        /// <summary>
        /// 刷新过期时间
        /// </summary>
        public int? refreshTokenexp { get; set; } = 0;

        /// <summary>
        ///成功
        /// </summary>

        public bool success { get; set; } = false;
    }
}