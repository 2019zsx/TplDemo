using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Roles")]
    public partial class RoleEntity
    {
        [SugarColumn(IsIgnore = false, IsPrimaryKey = true, IsIdentity = true)]
        /// <summary></summary>
        public int ID { get; set; }

        /// <summary>角色名称</summary>
        public string RoleName { get; set; }

        /// <summary>是否启用</summary>

        public bool IsDeleted { get; set; }
    }
}