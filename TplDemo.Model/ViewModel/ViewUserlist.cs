using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary>用户列表</summary>
    public class ViewUserlist
    {
        /// <summary></summary>
        public int Id { get; set; }

        /// <summary>登录名称</summary>
        public string LoginName { get; set; }

        /// <summary>用户名</summary>
        public string UserName { get; set; }

        /// <summary>性别</summary>
        public int? Sex { get; set; }

        /// <summary>出生日期</summary>
        public DateTime? Birthday { get; set; }

        /// <summary>手机号</summary>
        public string Phone { get; set; }

        /// <summary></summary>
        public string Email { get; set; }

        /// <summary>是否启用</summary>

        public bool IsDelete { get; set; }
        /// <summary>角色信息</summary>

        public string RoleName { get; set; }
    }
}