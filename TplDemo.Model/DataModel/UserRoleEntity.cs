using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    ///用户角色关系表
    ///</summary>
    [SugarTable("UserRoles")]
    public partial class UserRoleEntity
    {
        public UserRoleEntity()
        {
        }
        /// <summary></summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>用户Id</summary>
        public int UserID { get; set; }

        /// <summary>角色Id</summary>
        public int RoleID { get; set; }

    
    }
}