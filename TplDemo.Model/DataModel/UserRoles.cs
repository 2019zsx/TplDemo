using System;
using System.Linq;
using System.Text;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    ///用户角色关系表
    ///</summary>
    public partial class UserRoles
    {
        public UserRoles()
        {
        }

        /// <summary>用户Id</summary>
        public int UserID { get; set; }

        /// <summary>角色Id</summary>
        public int RoleID { get; set; }

        /// <summary></summary>
        public int Id { get; set; }
    }
}