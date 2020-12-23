using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Repository.BASE;

namespace TplDemo.Services
{
    public class RoleServices : BASE.BaseServices<RoleEntity>, RoleIServices
    {
        public IBaseRepository<RoleEntity> dal;
        public IBaseRepository<UserRoleEntity> dbUserRole;

        public RoleServices(IBaseRepository<RoleEntity> _dal, IBaseRepository<UserRoleEntity> _dbUserRole)
        {
            dal = _dal;
            base.BaseDal = dal;
            dbUserRole = _dbUserRole;
        }

        public async Task<bool> Isuserrole(int roleid, int uid)
        {
            var userroledata = await dbUserRole.Query(c => c.RoleID == roleid && c.UserID == uid);

            return userroledata.Count == 0 ? false : true;
        }

        /// <summary>获取当前用户当前所有角色</summary>
        /// <param name="roleId"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<List<UserRoleEntity>> GetdbUserRole(int uid)
        {
            return await dbUserRole.Query(c => c.UserID == uid);
        }

        /// <summary>获取角色名称</summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public Task<string> Getrolename(int roleid)
        {
            return dal.GetString("select RoleName from Roles where id=@roleid", new { roleid = roleid });
        }
    }
}