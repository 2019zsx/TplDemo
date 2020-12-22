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
        public RolePermissions()
        {
        }

        /// <summary></summary>

        public int Id { get; set; }

        /// <summary>角色ID</summary>
        public int RoleID { get; set; }

        /// <summary>权限ID</summary>
        public int PermissionID { get; set; }

        public bool? IsDeleted { get; set; }
    }
}