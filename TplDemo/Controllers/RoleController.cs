using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Common.Message;
using TplDemo.IServices;
using TplDemo.Model.DataModel;
using TplDemo.Model.ViewModel;
using SqlSugar;
using TplDemo.Common.IsWhatExtenions;

namespace TplDemo.Controllers
{
    /// <summary>角色信息</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region

        private RoleIServices roleIServices { get; set; }
        /// <summary></summary>

        private readonly IMapper mapper;
        /// <summary></summary>
        /// <param name="_mapper"></param>
        /// <param name="_roleIServices"></param>

        public RoleController(IMapper _mapper, RoleIServices _roleIServices)
        {
            roleIServices = _roleIServices;
            mapper = _mapper;
        }

        #endregion

        #region 获取角色集合

        /// <summary>获取角色集合</summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<List<RoleEntity>>> GetRoleList()
        {
            return new PageModel<List<RoleEntity>>()
            {
                data = await roleIServices.Query(c => c.IsDeleted == false)
            };
        }

        #endregion 获取角色集合

        #region 获取角色列表

        /// <summary>获取角色列表</summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<List<RoleEntity>>> GetRolePage(int PageIndex = 1, int PageSize = 10, string search = "")
        {
            var userlist = await roleIServices.QueryPage(null, PageIndex, PageSize, "id desc");
            return new PageModel<List<RoleEntity>>()
            {
                data = await roleIServices.Query(c => c.IsDeleted == false)
            };
        }

        #endregion 获取角色列表

        #region 添加角色

        /// <summary>添加角色</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<object>> Create(ViewCreateRole model)
        {
            var pageModel = new PageModel<object>();
            if (model.roleName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写角色名称";

                return pageModel;
            }
            int id = await roleIServices.Add(new RoleEntity() { RoleName = model.roleName, IsDeleted = false });
            if (id == 0)
            {
                pageModel.state = 30002;
                pageModel.msg = "添加角色失败";

                return pageModel;
            }
            return pageModel;
        }

        #endregion 添加角色

        #region 编辑角色

        /// <summary>编辑角色</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<PageModel<object>> Edit(ViewEditRole model)
        {
            var pageModel = new PageModel<object>();
            if (model.roleName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写角色名称";

                return pageModel;
            }
            bool msg = await roleIServices.Update(new RoleEntity() { ID = model.id, RoleName = model.roleName, IsDeleted = false });
            if (!msg)
            {
                pageModel.state = 30002;
                pageModel.msg = "编辑角色失败";

                return pageModel;
            }
            return pageModel;
        }

        #endregion 编辑角色

        #region 删除角色

        /// <summary>删除角色</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<PageModel<object>> Del(int id)
        {
            var pageModel = new PageModel<object>();

            bool msg = await roleIServices.DeleteById(id);
            if (!msg)
            {
                pageModel.state = 30002;
                pageModel.msg = "删除角色失败";

                return pageModel;
            }
            return pageModel;
        }

        #endregion 删除角色
    }
}