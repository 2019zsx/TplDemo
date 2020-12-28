using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Common.Message;
using TplDemo.IServices;
using TplDemo.Model.DataModel;

namespace TplDemo.Controllers
{
    /// <summary>角色信息</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private RoleIServices roleIServices { get; set; }
        /// <summary></summary>
        /// <param name="_roleIServices"></param>

        public RoleController(RoleIServices _roleIServices)
        {
            roleIServices = _roleIServices;
        }

        /// <summary>获取角色列表</summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModel<List<RoleEntity>>> GetRoleList()
        {
            return new PageModel<List<RoleEntity>>()
            {
                data = await roleIServices.Query(c => c.IsDeleted == false)
            };
        }
    }
}