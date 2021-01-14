using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("RolePermissions")]
    public partial class RolePermissions
    {
        /// <summary></summary>
        public RolePermissions()
        {
        }

        /// <summary></summary>
        [SugarColumn(IsIgnore = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>角色ID</summary>
        public int RoleID { get; set; }

        /// <summary>权限ID</summary>
        public int PermissionID { get; set; }

        /// <summary></summary>

        public bool? IsDeleted { get; set; }
    }
}