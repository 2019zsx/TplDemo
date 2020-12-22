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

            var permissionTrees = (from child in Permissionlist

                                   orderby child.ID
                                   select new ViewMenuTree
                                   {
                                       id = child.ID,
                                       name = child.Name,
                                       pid = child.ParentID.ObjToInt(),
                                       order = child.orderID.ObjToInt(),
                                       path = child.Path,
                                       iconCls = child.Icon,

                                       component = child.Path,
                                       //Func = child.Func,
                                       hidden = child.IsButton.ObjToBool(),
                                       //IsButton = child.IsButton.ObjToBool(),
                                       meta = new meta
                                       {
                                           affix = true,
                                           //icon = child.Icon,
                                           icon = "dashboard",
                                           title = child.Name
                                           // requireAuth = true, title = child.Name, NoTabPage =
                                           // child.IsHide.ObjToBool(), keepAlive = child.IskeepAlive.ObjToBool()
                                       }
                                   }).ToList();

            ViewMenuTree rootRoot = new ViewMenuTree()
            {
                id = 0,
                pid = 0,
                order = 0,
                name = "根节点",
                path = "",
                iconCls = "",
                meta = new meta(),
            };
            permissionTrees = permissionTrees.OrderBy(d => d.order).ToList();
            LoopNaviBarAppendChildren(permissionTrees, rootRoot);
            return permissionTrees;
        }

        public static void LoopNaviBarAppendChildren(List<ViewMenuTree> all, ViewMenuTree curItem)
        {
            var subItems = all.Where(ee => ee.pid == curItem.id).ToList();

            if (subItems.Count > 0)
            {
                curItem.children = new List<ViewMenuTree>();
                curItem.children.AddRange(subItems);
            }
            else
            {
                curItem.children = null;
            }

            foreach (var subItem in subItems)
            {
                LoopNaviBarAppendChildren(all, subItem);
            }
        }
    }
}