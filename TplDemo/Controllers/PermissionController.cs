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
using TplDemo.Model.ViewModel;

namespace TplDemo.Controllers
{
    /// <summary>权限管理</summary>
    [Route("api/[controller]")]
    [ApiController]
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

        /// <summary>获取菜单</summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetMenuTree")]
        [Authorize]
        public async Task<PageModel<List<ViewMenuTree>>> GetMenuTree()
        {
            var data = await dbpermissionIServices.GetMenuTree(user.role);
            data = data.OrderBy(c => c.id).ToList();
            return new PageModel<List<ViewMenuTree>>() { data = data };
        }
    }
}