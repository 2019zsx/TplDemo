using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary></summary>
    public class ViewCreateUser
    {
        /// <summary>用户名</summary>
        public string LoginName { get; set; }

        /// <summary>密码</summary>
        public string Password { get; set; }

        /// <summary>姓名</summary>
        public string UserName { get; set; }

        /// <summary>性别 e</summary>
        public bool? Sex { get; set; }

        /// <summary>出生日期</summary>
        public DateTime? Birthday { get; set; }

        /// <summary>手机号</summary>
        public string Phone { get; set; }

        /// <summary>e</summary>
        public string Email { get; set; }

        /// <summary>角色Id集合</summary>

        public string roleId { get; set; }
    }
}