using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Common.Message;
using TplDemo.IServices;
using TplDemo.Model.ViewModel;
using TplDemo.Common.IsWhatExtenions;
using TplDemo.Model.DataModel;
using TplDemo.Common.HttpContextUser;
using SqlSugar;
using TplDemo.Repository.UnitOfWork;
using TplDemo.Common.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace TplDemo.Controllers
{
    /// <summary>用户信息</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SysUserController : ControllerBase
    {
        /// <summary>获取用户信息</summary>
        private sysUserInfoIServices dbsysUserInfoIServices;

        private IUser user;

        private UserRoleIServices userRoleIServices;

        private RoleIServices roleIServices;
        private readonly IMapper mapper;

        /// <summary></summary>
        public IUnitOfWork unitOfWork;

        /// <summary></summary>
        /// <param name="_user"></param>
        /// <param name="_mapper"></param>
        /// <param name="_unitOfWork"></param>
        /// <param name="_dbsysUserInfoIServices"></param>
        /// <param name="_roleIServices"></param>
        /// <param name="_userRoleIServices"></param>
        public SysUserController(IUser _user, IMapper _mapper, IUnitOfWork _unitOfWork, sysUserInfoIServices _dbsysUserInfoIServices, RoleIServices _roleIServices, UserRoleIServices _userRoleIServices)
        {
            user = _user;
            dbsysUserInfoIServices = _dbsysUserInfoIServices;
            roleIServices = _roleIServices;
            userRoleIServices = _userRoleIServices;
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        #region 获取用户列表

        /// <summary>获取用户列表</summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<List<sysUserInfoEntity>>> GetUserPage(int PageIndex = 1, int PageSize = 10, string search = "")
        {
            var userlist = await dbsysUserInfoIServices.QueryPage(null, PageIndex, PageSize, "id desc");

            foreach (var item in userlist.data)
            {
                var roleuserdata = await roleIServices.GetdbUserRole(item.Id);
                var rolearr = roleuserdata.Select(c => c.RoleID).ToArray();
                var roledata = await roleIServices.Query(c => rolearr.Contains(c.ID));
                item.roleName = string.Join(",", roledata.Select(c => c.roleName).ToArray());
            }
            return userlist;
        }

        #endregion 获取用户列表

        #region 获取用户信息

        /// <summary>获取用户信息</summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<sysUserInfoEntity>> GetUser(int id)
        {
            var userlist = await dbsysUserInfoIServices.Query(c => c.Id == id && c.isDeleted == false);
            var usermodel = userlist.FirstOrDefault();
            if (usermodel != null)
            {
                var roleuserdata = await roleIServices.GetdbUserRole(usermodel.Id);
                usermodel.roleId = roleuserdata.Select(c => c.RoleID).ToArray();
            }
            else
            {
                return new PageModel<sysUserInfoEntity>() { state = 403, msg = "当前用户不存在" };
            }
            return new PageModel<sysUserInfoEntity>() { data = usermodel };
        }

        #endregion 获取用户信息

        #region 创建用户

        /// <summary>创建用户</summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<PageModel<object>> Create(ViewCreateUser model)
        {
            var pageModel = new PageModel<object>();
            if (model.loginName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写登录名称";
                return pageModel;
            }
            var userdata = await dbsysUserInfoIServices.Query(c => c.LoginName == model.loginName);
            if (userdata.Count > 0)
            {
                pageModel.state = 30002;
                pageModel.msg = "当前登录名已存在！";
                return pageModel;
            }
            if (model.userName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写姓名";
                return pageModel;
            }

            if (model.password.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写密码";
                return pageModel;
            }
            model.password = MD5Helper.MD5Encrypt32(model.password);
            if (model.roleId.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填选择角色";
                return pageModel;
            }
            try
            {
                unitOfWork.BeginTranReadUncommitted();
                int uid = await dbsysUserInfoIServices.Add(new sysUserInfoEntity()
                {
                    Age = model.age,
                    Email = model.email,
                    isDeleted = model.isDeleted,
                    LoginName = model.loginName,
                    Password = model.password,
                    Phone = model.phone,
                    Sex = model.sex,
                    UserName = model.userName
                });
                if (uid == 0)
                {
                    throw new Exception("添加用户失败");
                }
                // await userRoleIServices.DeleteByWhere(c => c.UserID == uid); int[] roleidarr =
                // model.roleId.Split(',').Select(c => c.ObjToInt()).ToArray();
                int[] roleidarr = model.roleId.ToArray();
                List<UserRoleEntity> list = new List<UserRoleEntity>();
                for (int i = 0; i < roleidarr.Length; i++)
                {
                    list.Add(new UserRoleEntity() { RoleID = roleidarr[i], UserID = uid });
                }
                int msg = await userRoleIServices.Add(list);
                if (msg == 0)
                {
                    throw new Exception("添加用户失败");
                }
                unitOfWork.CommitTranUncommitted();
            }
            catch (Exception ex)
            {
                unitOfWork.RollbackTranUncommitted();
                pageModel.state = 30002;
                pageModel.msg = ex.Message;
                return pageModel;
            }
            return pageModel;
        }

        #endregion 创建用户

        #region 编辑用户

        /// <summary>编辑用户</summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        public async Task<PageModel<object>> Edit(ViewEditUser model)
        {
            var pageModel = new PageModel<object>();
            if (model.userName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写姓名";
                return pageModel;
            }

            if (model.password.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写密码";
                return pageModel;
            }
            var userdata = await dbsysUserInfoIServices.QueryById(model.id);
            if (userdata.Password != model.password)
            {
                model.password = MD5Helper.MD5Encrypt32(model.password);
            }

            if (model.roleId.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填选择角色";
                return pageModel;
            }
            try
            {
                unitOfWork.BeginTranReadUncommitted();
                bool msg = await dbsysUserInfoIServices.Update(new sysUserInfoEntity()
                {
                    Id = model.id,
                    Age = model.age,
                    Email = model.email,
                    isDeleted = model.isDeleted,
                    LoginName = userdata.LoginName,
                    Password = model.password,
                    Phone = model.phone,
                    Sex = model.sex,
                    UserName = model.userName
                });
                if (!msg)
                {
                    throw new Exception("修改用户失败");
                }

                int[] roleidarr = model.roleId.ToArray();
                // 先删除现有的用户角色
                await userRoleIServices.DeleteByWhere(c => c.UserID == model.id);
                List<UserRoleEntity> list = new List<UserRoleEntity>();
                for (int i = 0; i < roleidarr.Length; i++)
                {
                    list.Add(new UserRoleEntity() { RoleID = roleidarr[i], UserID = model.id });
                }
                int roleid = await userRoleIServices.Add(list);
                if (roleid == 0)
                {
                    throw new Exception("修改用户失败");
                }
                unitOfWork.CommitTranUncommitted();
            }
            catch (Exception ex)
            {
                unitOfWork.RollbackTranUncommitted();
                pageModel.state = 30002;
                pageModel.msg = ex.Message;
                return pageModel;
            }
            return pageModel;
        }

        #endregion 编辑用户

        #region 删除用户

        /// <summary>删除</summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete]
        public async Task<PageModel<object>> Del(int id)
        {
            var pageModel = new PageModel<object>();
            try
            {
                unitOfWork.BeginTranReadUncommitted();
                var msg = await dbsysUserInfoIServices.DeleteById(id);
                if (!msg)
                {
                    throw new Exception("删除用户失败");
                }
                // 先删除现有的用户
                msg = await userRoleIServices.DeleteByWhere(c => c.UserID == id);
                if (!msg)
                {
                    throw new Exception("删除用户失败");
                }
                unitOfWork.CommitTranUncommitted();
            }
            catch (Exception ex)
            {
                unitOfWork.RollbackTranUncommitted();
                pageModel.state = 30002;
                pageModel.msg = ex.Message;
                return pageModel;
            }
            return pageModel;
        }

        #endregion 删除用户

        #region 更改用户状态

        /// <summary>更改用户状态</summary>
        /// <param name="id"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<PageModel<object>> isDelete(int id, bool isDelete)
        {
            var pageModel = new PageModel<object>();
            var msg = await dbsysUserInfoIServices.Update(new sysUserInfoEntity() { Id = id, isDeleted = isDelete }, new List<string>() { "Id", "IsDelete" });
            if (!msg)
            {
                pageModel.state = 30002;
                pageModel.msg = "修改失败";
                return pageModel;
            }
            return pageModel;
        }

        #endregion 更改用户状态
    }
}