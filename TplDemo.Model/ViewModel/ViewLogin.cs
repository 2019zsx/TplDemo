using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary></summary>
    public class ViewLogin
    {
        /// <summary>用户名</summary>
        [Required(ErrorMessage = "用户名不能为空"), MaxLength(10, ErrorMessage = "名字不能超过10字符"), MinLength(0, ErrorMessage = "名字太短")]
        public string uloginname { get; set; }

        /// <summary>密码</summary>
        [Required(ErrorMessage = "密码不能为空"), MaxLength(10, ErrorMessage = "密码"), MinLength(0, ErrorMessage = "名字太短")]
        public string updw { get; set; }

        /// <summary>角色Id</summary>
        public int roleid { get; set; } = 0;

        /// <summary>
        ///登录类型
        /// </summary>
        public string requesttype { get; set; } = "web";
    }
}