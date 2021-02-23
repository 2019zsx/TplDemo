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

        /// <summary>角色名称</summary>
        [SugarColumn(ColumnName = "RoleName")]
        public string roleName { get; set; }

        /// <summary>描述</summary>

        [SugarColumn(ColumnName = "describe")]
        public string describe { get; set; }

        /// <summary>是否启用</summary>
        [SugarColumn(ColumnName = "IsDeleted")]
        public bool isDeleted { get; set; }

        /// <summary>添加时间</summary>
        [SugarColumn(ColumnName = "createTime")]
        public DateTime? createTime { get; set; }
    }
}