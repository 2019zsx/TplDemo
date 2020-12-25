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

namespace TplDemo.Controllers
{
    /// <summary>用户信息</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SysUserController : ControllerBase
    {
        /// <summary>获取用户信息</summary>
        private sysUserInfoIServices dbsysUserInfoIServices;

        private UserRoleIServices UserRoleIServices;

        /// <summary></summary>
        /// <param name="_dbsysUserInfoIServices"></param>
        public SysUserController(sysUserInfoIServices _dbsysUserInfoIServices)
        {
            dbsysUserInfoIServices = _dbsysUserInfoIServices;
        }

        /// <summary>创建用户</summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<PageModel<object>> Create(ViewCreateUser model)
        {
            var pageModel = new PageModel<object>();
            if (model.UserName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写姓名";
                return pageModel;
            }
            if (model.LoginName.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写用户名";
                return pageModel;
            }
            if (model.Password.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填写密码";
                return pageModel;
            }
            if (model.roleId.IsNullOrEmpty())
            {
                pageModel.state = 30002;
                pageModel.msg = "请填选择角色";
                return pageModel;
            }
            // dbsysUserInfoIServices.Add();
            return null;
        }
    }
}