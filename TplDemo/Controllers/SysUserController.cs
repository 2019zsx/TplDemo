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

namespace TplDemo.Controllers
{
    /// <summary>用户信息</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
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

        /// <summary>获取用户信息</summary>
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
                item.roleName = string.Join(",", roledata.Select(c => c.RoleName).ToArray());
            }
            return userlist;
        }

        /// <summary>获取用户信息</summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<sysUserInfoEntity>> GetUser(int id)
        {
            var userlist = await dbsysUserInfoIServices.QueryById(id);
            if (userlist != null)
            {
                var roleuserdata = await roleIServices.GetdbUserRole(userlist.Id);
                userlist.roleId = roleuserdata.Select(c => c.RoleID).ToArray();
            }
            return new PageModel<sysUserInfoEntity>() { data = userlist };
        }

        /// <summary>创建用户</summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<PageModel<object>> Create(ViewCreateUser model)
        {
            var dto = mapper.Map<sysUserInfoEntity>(model);
            var pageModel = new PageModel<object>();
            if (model.LoginName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写登录名称";
                return pageModel;
            }
            var userdata = await dbsysUserInfoIServices.Query(c => c.LoginName == model.LoginName);
            if (userdata.Count > 0)
            {
                pageModel.state = 30002;
                pageModel.msg = "当前登录名已存在！";
                return pageModel;
            }
            if (model.UserName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写姓名";
                return pageModel;
            }

            if (model.Password.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写密码";
                return pageModel;
            }
            model.Password = MD5Helper.MD5Encrypt32(model.Password);
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
                    Age = model.Age,
                    Email = model.Email,
                    IsDelete = model.IsDelete,
                    LoginName = model.LoginName,
                    Password = model.Password,
                    Phone = model.Phone,
                    Sex = model.Sex,
                    UserName = model.UserName
                });
                if (uid == 0)
                {
                    throw new Exception("添加用户失败");
                }
                int[] roleidarr = model.roleId.Split(',').Select(c => c.ObjToInt()).ToArray();
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
    }
}