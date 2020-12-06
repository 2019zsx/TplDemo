using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary>
    ///
    /// </summary>
    public class Viewtrefreshtoken
    {
        /// <summary>
        /// 验证token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string refreshtoken { get; set; }
    }
}