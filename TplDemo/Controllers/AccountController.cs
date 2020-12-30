using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TplDemo.Common.Helper;
using TplDemo.Common.IsWhatExtenions;
using TplDemo.Common.Message;
using TplDemo.Extensions.Authorization;
using TplDemo.IServices;
using System.Linq.Expressions;
using TplDemo.Model.ViewModel;
using TplDemo.Model.DataModel;
using Microsoft.AspNetCore.Authorization;
using TplDemo.Common.HttpContextUser;
using System.Security.Claims;
using TplDemo.Common.TokenModel;
using SqlSugar;

namespace TplDemo.Controllers
{
    /// <summary>用户登录信息</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>获取用户信息</summary>
        private sysUserInfoIServices dbsysUserInfoIServices;

        /// <summary>获取角色信息</summary>
        private RoleIServices dbRoleIServices;

        private IUser dbUse;

        /// <summary></summary>
        /// <param name="_dbsysUserInfoIServices"></param>
        /// <param name="_dbRoleIServices"></param>
        /// <param name="_dbUse"></param>
        public AccountController(sysUserInfoIServices _dbsysUserInfoIServices, RoleIServices _dbRoleIServices, IUser _dbUse)
        {
            dbsysUserInfoIServices = _dbsysUserInfoIServices;
            dbRoleIServices = _dbRoleIServices;
            dbUse = _dbUse;
        }

        /// <summary>系统登录</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<ViewToken>> SystemLogin(ViewLogin model)
        {
            var pageModel = new PageModel<ViewToken>();
            if (model.uloginname.IsNullOrEmpty())
            {
                //pageModel.state = 30002;
                pageModel.success = false;
                pageModel.msg = "请填写用户名";
                return pageModel;
            }
            if (model.updw.IsNullOrEmpty())
            {
                //pageModel.state = 30002;
                pageModel.success = false;
                pageModel.msg = "请填写密码";
                return pageModel;
            }

            string pdw = MD5Helper.MD5Encrypt32(model.updw);
            var userdata = await dbsysUserInfoIServices.Query(c => c.IsDelete == false && c.LoginName == model.uloginname && c.Password == pdw);
            if (userdata.Count == 0)
            {
                pageModel.success = false;
                pageModel.msg = "当用户名或者密码错误！";
                return pageModel;
            }
            var usermodel = userdata.FirstOrDefault();
            // 判断当前的选择的角色和用户是否存在
            var isuserrole = await dbRoleIServices.Isuserrole(model.roleid, usermodel.Id);
            if (!isuserrole)
            {
                pageModel.success = false;
                pageModel.msg = "登录失败";
                return pageModel;
            }
            pageModel.data = JwtHelper.GetToken(new Common.TokenModel.Userinfo() { roleid = model.roleid, uid = usermodel.Id, username = usermodel.UserName }, "web");
            return pageModel;
        }

        /// <summary>获取角色信息</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<List<VIewLoginRolelist>>> GetLoginRole(ViewLoginRole model)
        {
            var pageModel = new PageModel<List<VIewLoginRolelist>>();
            if (model.uloginname.IsNullOrEmpty())
            {
                //pageModel.state = 30002;
                pageModel.success = false;
                pageModel.msg = "请填写用户名";
                return pageModel;
            }
            if (model.updw.IsNullOrEmpty())
            {
                //pageModel.state = 30002;
                pageModel.success = false;
                pageModel.msg = "请填写密码";
                return pageModel;
            }

            string pdw = MD5Helper.MD5Encrypt32(model.updw);
            var userdata = await dbsysUserInfoIServices.Query(c => c.IsDelete == false && c.LoginName == model.uloginname && c.Password == pdw);
            if (userdata.Count == 0)
            {
                pageModel.success = false;
                pageModel.msg = "当用户名或者密码错误！";
                return pageModel;
            }
            var usermodel = userdata.FirstOrDefault();
            // 获取当前用户对应的角色信息
            var userroledata = await dbRoleIServices.GetdbUserRole(usermodel.Id);
            int[] roleidarr = userroledata.Select(c => c.Id).ToArray();
            Expression<Func<RoleEntity, VIewLoginRolelist>> selectexp = it => new VIewLoginRolelist() { roleid = it.ID, rolename = it.RoleName };
            var roeldata = await dbRoleIServices.Query(c => roleidarr.Contains(c.ID), selectexp, "");
            pageModel.data = roeldata;
            return pageModel;
        }

        /// <summary>获取用户信息</summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<PageModel<ViewUserinfo>> GetUserinfo()
        {
            int s = "0".ObjToInt();
            var pageModel = new PageModel<ViewUserinfo>();
            ViewUserinfo viewUserinfo = new ViewUserinfo();
            var usermodel = await dbsysUserInfoIServices.QueryById(dbUse.uid);
            if (usermodel == null)
            {
                // pageModel.state = 30002;
                pageModel.success = false;
                pageModel.msg = "当前用户不存在";
                return pageModel;
            }
            string rolename = await dbRoleIServices.Getrolename(dbUse.role);
            pageModel.data = new ViewUserinfo()
            {
                uid = usermodel.Id,
                username = usermodel.UserName,
                roleid = dbUse.role,
                rolename = rolename
            };
            return pageModel;
        }

        /// <summary>刷新token</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageModel<ViewToken>> Getrefreshtoken(Viewtrefreshtoken model)
        {
            var pageModel = new PageModel<ViewToken>() { };
            if (string.IsNullOrEmpty(model.access_token))
            {
                pageModel.success = false;
                pageModel.msg = "验证token不能为空";
                return pageModel;
            }
            if (string.IsNullOrEmpty(model.refreshtoken))
            {
                pageModel.success = false;
                pageModel.msg = "刷新token不能为空";
                return pageModel;
            }
            // 验证刷新token是否过期

            bool msg = JwtHelper.ValidateToken(model.refreshtoken);
            if (!msg)
            {
                pageModel.success = false;
                pageModel.msg = "登录超时";
                return pageModel;
            }
            var userinfo = dbUse.GetSerializeJwt(model.access_token);
            if (userinfo.uid == 0)
            {
                pageModel.success = false;
                pageModel.msg = "（验证）token无效";
                return pageModel;
            }
            var userdata = await dbsysUserInfoIServices.Query(c => c.IsDelete == false && c.Id == userinfo.uid);

            if (userdata == null)
            {
                pageModel.success = false;
                pageModel.msg = "（验证）token无效";
                return pageModel;
            }
            var isuserrole = await dbRoleIServices.Isuserrole(userinfo.roleid, userinfo.uid);
            if (!isuserrole)
            {
                pageModel.success = false;
                pageModel.msg = "（验证）token无效";
                return pageModel;
            }
            var jwt = JwtHelper.GetToken(new Userinfo { uid = userinfo.uid, roleid = userinfo.roleid, username = userinfo.username }, "web");
            jwt.refreshtoken = "";
            pageModel.data = jwt;
            return pageModel;
        }
    }
}