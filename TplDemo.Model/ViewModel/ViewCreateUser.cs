using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary></summary>
    public class ViewCreateUser
    {
        /// <summary>用户名</summary>
        public string loginName { get; set; }

        /// <summary>密码</summary>
        public string password { get; set; }

        /// <summary>姓名</summary>
        public string userName { get; set; }

        /// <summary>性别 e</summary>
        public int sex { get; set; }

        /// <summary>年龄</summary>
        public int age { get; set; }

        /// <summary>手机号</summary>
        public string phone { get; set; }

        /// <summary>e</summary>
        public string email { get; set; }

        /// <summary>角色Id集合</summary>

        public IEnumerable<int> roleId { get; set; }

        /// <summary></summary>
        public bool isDelete { get; set; }
    }
}