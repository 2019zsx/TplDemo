using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    public class ViewToken
    {
        /// <summary>
        ///验证token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        ///过期时间
        /// </summary>
        public DateTime expires_in { get; set; }

        /// <summary>
        ///刷新
        /// </summary>
        public string refreshtoken { get; set; }

        /// <summary>
        /// 消息
        /// </summary>

        public string msg { get; set; }
        /// <summary>
        ///成功
        /// </summary>

        public bool success { get; set; }
    }
}