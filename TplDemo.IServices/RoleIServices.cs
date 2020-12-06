using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Model.DataModel;

namespace TplDemo.IServices
{
    public interface RoleIServices : BASE.IBaseServices<RoleEntity>
    {
        Task<string> Getrolename(int roleid);

        Task<bool> Isuserrole(int roleid, int uid);

        Task<List<UserRoleEntity>> GetdbUserRole(int uid);
    }
}