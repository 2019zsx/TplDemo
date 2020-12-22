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
        public int ID { get; set; }

        /// <summary>角色Id</summary>
        public string RoleName { get; set; }
    }
}