using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Users")]
    public partial class sysUserInfoEntity
    {
        /// <summary></summary>
        public int Id { get; set; }

        /// <summary>用户名</summary>
        public string LoginName { get; set; }

        /// <summary>密码 e</summary>
        public string Password { get; set; }

        /// <summary>用户名 e</summary>
        public string UserName { get; set; }

        /// <summary>性别 e</summary>
        public bool? Sex { get; set; }

        /// <summary>出生日期 e</summary>
        public DateTime? Birthday { get; set; }

        /// <summary>手机号 e</summary>
        public string Phone { get; set; }

        /// <summary>e</summary>
        public string Email { get; set; }

        public bool IsDelete { get; set; }
    }
}