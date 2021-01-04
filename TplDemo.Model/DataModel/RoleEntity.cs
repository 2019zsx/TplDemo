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
        /// <summary></summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "ID")]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "RoleName")]

        /// <summary>角色名称</summary>
        public string roleName { get; set; }

        /// <summary>描述</summary>

        [SugarColumn(ColumnName = "describe")]
        public string describe { get; set; }

        [SugarColumn(ColumnName = "IsDeleted")]
        /// <summary>是否启用</summary>

        public bool isDeleted { get; set; }

        [SugarColumn(ColumnName = "createTime")]
        /// <summary>添加时间</summary>

        public DateTime? createTime { get; set; }
    }
}