using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Model.ViewModel;
using TplDemo.Repository.BASE;

namespace TplDemo.Services
{
    public class PermissionServices : BASE.BaseServices<Permission>, PermissionIServices
    {
        private IBaseRepository<Permission> dal;
        private IBaseRepository<RolePermissions> dalRoleModulePermission;

        public PermissionServices(IBaseRepository<Permission> _dal, IBaseRepository<RolePermissions> _dalRoleModulePermission)
        {
            dal = _dal;
            base.BaseDal = dal;
            dalRoleModulePermission = _dalRoleModulePermission;
        }

        /// <summary></summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public async Task<List<RolePermissions>> GetRoleModulePermission(int roleid)
        {
            return await dalRoleModulePermission.Query(c => c.RoleID == roleid && c.IsDeleted == false);
        }

        /// <summary>获取导航栏</summary>
        /// <param name="roleid">角色名称</param>
        /// <returns></returns>
        public async Task<List<ViewMenuTree>> GetMenuTree(int roleid)
        {
            var rmpdata = await GetRoleModulePermission(roleid);
            int[] rmparrid = rmpdata.Select(c => c.PermissionID.ObjToInt()).ToArray();
            var Permissionlist = await dal.Query(c => rmparrid.Contains(c.ID));

            return GetMenuTreelist(Permissionlist);
        }

        /// <summary>获取所有</summary>
        /// <returns></returns>
        public async Task<List<ViewMenuTree>> GetMenuTreeAll()
        {
            var Permissionlist = await dal.Query();
            return GetMenuTreelist(Permissionlist);
        }

        public List<ViewMenuTree> GetMenuTreelist(List<Permission> Permissionlist)
        {
            var MenuTreedata = Permissionlist.Where(c => c.ParentID == 0).ToList();
            List<ViewMenuTree> permissionTrees = new List<ViewMenuTree>();
            foreach (var child in MenuTreedata)
            {
                ViewMenuTree menuTree = new ViewMenuTree();

                menuTree.id = child.ID;
                menuTree.name = child.Name;
                menuTree.pid = child.ParentID.ObjToInt();
                menuTree.order = child.orderID.ObjToInt();
                menuTree.path = child.Path;
                menuTree.iconCls = child.Icon;
                menuTree.component = child.component;
                //Func = child.Func,
                menuTree.hidden = child.IsButton.ObjToBool();
                menuTree.meta = new meta
                {
                    affix = true,
                    //icon = child.Icon,
                    icon = "dashboard",
                    title = child.Title
                    // requireAuth = true, title = child.Name, NoTabPage = child.IsHide.ObjToBool(),
                    // keepAlive = child.IskeepAlive.ObjToBool()
                };
                menuTree.children = GetMenuTreeChildren(Permissionlist, child.ID);
                permissionTrees.Add(menuTree);
            }

            return permissionTrees;
        }

        /// <summary>获取菜单树</summary>
        /// <param name="id">顶级ID</param>
        /// <returns></returns>
        public List<ViewMenuTree> GetMenuTreeChildren(List<Permission> all, int ParentID)
        {
            List<ViewMenuTree> data = new List<ViewMenuTree>();

            var MenuTreedata = all.Where(ee => ee.ParentID == ParentID).ToList(); ;
            foreach (var child in MenuTreedata)
            {
                ViewMenuTree menuTree = new ViewMenuTree();

                menuTree.id = child.ID;
                menuTree.name = child.Name;
                menuTree.pid = child.ParentID.ObjToInt();
                menuTree.order = child.orderID.ObjToInt();
                menuTree.path = child.Path;
                menuTree.iconCls = child.Icon;
                menuTree.component = child.component;
                //Func = child.Func,
                menuTree.hidden = child.IsButton.ObjToBool();
                //IsButton = child.IsButton.ObjToBool(),
                menuTree.meta = new meta
                {
                    affix = true,
                    icon = "dashboard",
                    title = child.Title
                };
                data.Add(menuTree);
            }

            return data;
        }
    }
}