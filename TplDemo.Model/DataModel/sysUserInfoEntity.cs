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
        [SugarColumn(IsIgnore = false, IsPrimaryKey = true, IsIdentity = true)]
        /// <summary></summary>
        public int Id { get; set; }

        /// <summary>用户名</summary>
        public string LoginName { get; set; }

        /// <summary>密码 e</summary>
        public string Password { get; set; }

        /// <summary>用户名 e</summary>
        public string UserName { get; set; }

        /// <summary>性别 e</summary>
        public int? Sex { get; set; }

        /// <summary>年龄</summary>
        public int Age { get; set; }

        /// <summary>手机号 e</summary>
        public string Phone { get; set; }

        /// <summary>e</summary>
        public string Email { get; set; }

        public bool isDeleted { get; set; }
        /// <summary>角色名称</summary>

        [SugarColumn(IsIgnore = true)]
        public string roleName { get; set; }

        /// <summary></summary>

        [SugarColumn(IsIgnore = true)]
        public int[] roleId { get; set; }
    }
}