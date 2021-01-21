using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TplDemo.Common.HttpContextUser;
using TplDemo.Common.Message;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Model.ViewModel;

namespace TplDemo.Controllers
{
    /// <summary>权限管理</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        private IUser user;
        private PermissionIServices dbpermissionIServices;

        /// <summary></summary>
        /// <param name="_user"></param>
        /// <param name="_dbpermissionIServices"></param>
        public PermissionController(IUser _user, PermissionIServices _dbpermissionIServices)
        {
            user = _user;
            dbpermissionIServices = _dbpermissionIServices;
        }

        #region 根据角色获取菜单

        /// <summary>根据角色获取菜单</summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<PageModel<List<ViewMenuTree>>> GetMenuTree()
        {
            var data = await dbpermissionIServices.GetMenuTree(user.role);
            data = data.OrderBy(c => c.id).ToList();
            return new PageModel<List<ViewMenuTree>>() { data = data };
        }

        #endregion 根据角色获取菜单

        #region 获取所有菜单

        /// <summary>获取所有菜单</summary>
        [HttpGet]
        public async Task<PageModel<List<ViewMenuTree>>> GetMenuTreeAll()
        {
            var data = await dbpermissionIServices.GetMenuTreeAll();
            return new PageModel<List<ViewMenuTree>>() { data = data };
        }

        #endregion 获取所有菜单

        #region 添加菜单

        /// <summary>添加菜单</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<object>> Create(ViewCreatePermission model)
        {
            var pageModel = new PageModel<object>();
            var msg = await dbpermissionIServices.Add(new Permission()
            {
                component = model.component,
                Icon = model.icon,
                IsButton = model.isButton,
                isEnable = model.isEnable,
                Name = model.component,
                orderID = model.orderID,
                ParentID = model.parentID,
                Path = model.path,
                Title = model.title
            }) > 0;
            if (!msg)
            {
                pageModel.state = 30002;
                pageModel.msg = "添加失败";
                return pageModel;
            }
            return pageModel;
        }

        #endregion 添加菜单

        #region 获取菜单详情信息

        /// <summary>获取菜单详情信息</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<Permission>> Getdetails(int id)
        {
            return new PageModel<Permission>() { data = await dbpermissionIServices.QueryById(id) };
        }

        #endregion 获取菜单详情信息

        #region 编辑菜单

        /// <summary>编辑菜单</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<PageModel<object>> Edit(ViewEditPermission model)
        {
            var pageModel = new PageModel<object>();
            var msg = await dbpermissionIServices.Update(new Permission()
            {
                ID = model.id,
                component = model.component,
                Icon = model.icon,
                IsButton = model.isButton,
                isEnable = model.isEnable,
                Name = model.component,
                orderID = model.orderID,
                ParentID = model.parentID,
                Path = model.path,
                Title = model.title
            });
            if (!msg)
            {
                pageModel.state = 30002;
                pageModel.msg = "编辑失败";
                return pageModel;
            }
            return pageModel;
        }

        #endregion 编辑菜单

        #region 删除菜单

        /// <summary>删除菜单</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<PageModel<object>> Del(int id)
        {
            var pageModel = new PageModel<object>();
            var msg = await dbpermissionIServices.DeleteById(id);
            if (!msg)
            {
                pageModel.state = 30002;
                pageModel.msg = "删除失败";
                return pageModel;
            }
            return pageModel;
        }

        #endregion 删除菜单
    }
}