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
        private IBaseRepository<RoleModulePermission> dalRoleModulePermission;

        public PermissionServices(IBaseRepository<Permission> _dal, IBaseRepository<RoleModulePermission> _dalRoleModulePermission)
        {
            dal = _dal;
            base.BaseDal = dal;
            dalRoleModulePermission = _dalRoleModulePermission;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public async Task<List<RoleModulePermission>> GetRoleModulePermission(int roleid)
        {
            return await dalRoleModulePermission.Query(c => c.RoleId == roleid && c.IsDeleted == false);
        }

        /// <summary>
        /// 获取导航栏
        /// </summary>
        /// <param name="roleid">角色名称</param>
        /// <returns></returns>
        public async Task<List<ViewMenuTree>> GetMenuTree(int roleid)
        {
            var rmpdata = await GetRoleModulePermission(roleid);
            int[] rmparrid = rmpdata.Select(c => c.PermissionId.ObjToInt()).ToArray();
            var Permissionlist = await dal.Query(c => rmparrid.Contains(c.Id) && c.IsButton == false && c.Enabled == false && c.IsDeleted == false);

            var permissionTrees = (from child in Permissionlist
                                   where child.IsDeleted == false
                                   orderby child.Id
                                   select new ViewMenuTree
                                   {
                                       id = child.Id,
                                       name = child.Name,
                                       pid = child.Pid,
                                       order = child.OrderSort,
                                       path = child.Code,
                                       iconCls = child.Icon,

                                       component = child.Code,
                                       //Func = child.Func,
                                       hidden = child.IsHide.ObjToBool(),
                                       //IsButton = child.IsButton.ObjToBool(),
                                       meta = new meta
                                       {
                                           affix = true,
                                           //icon = child.Icon,
                                           icon = "dashboard",
                                           title = child.Name
                                           // requireAuth = true,
                                           //  title = child.Name,
                                           //  NoTabPage = child.IsHide.ObjToBool(),
                                           //  keepAlive = child.IskeepAlive.ObjToBool()
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