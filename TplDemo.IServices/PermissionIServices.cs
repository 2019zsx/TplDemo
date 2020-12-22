using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Model.DataModel;
using TplDemo.Model.ViewModel;

namespace TplDemo.IServices
{
    public interface PermissionIServices : BASE.IBaseServices<Permission>
    {
        /// <summary>根据角色获取对应的权限Id</summary>
        /// <param name="roleid"></param>
        /// <returns></returns>

        Task<List<RolePermissions>> GetRoleModulePermission(int roleid);

        /// <summary>获取路由数据</summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        Task<List<ViewMenuTree>> GetMenuTree(int roleid);
    }
}