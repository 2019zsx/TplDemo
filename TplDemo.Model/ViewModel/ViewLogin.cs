using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    public class ViewLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string uloginname { get; set; }

        /// <summary>
        ///  密码
        /// </summary>

        public string updw { get; set; }

        /// <summary>
        ///  角色Id
        /// </summary>
        public int roleid { get; set; } = 0;

        /// <summary>
        ///登录类型
        /// </summary>
        public string requesttype { get; set; } = "web";
    }
}