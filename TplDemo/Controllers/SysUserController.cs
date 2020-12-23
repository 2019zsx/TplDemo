using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.IServices;

namespace TplDemo.Controllers
{
    /// <summary>用户信息</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SysUserController : ControllerBase
    {
        /// <summary>获取用户信息</summary>
        private sysUserInfoIServices dbsysUserInfoIServices;

        /// <summary></summary>
        /// <param name="_dbsysUserInfoIServices"></param>
        public SysUserController(sysUserInfoIServices _dbsysUserInfoIServices)
        {
            dbsysUserInfoIServices = _dbsysUserInfoIServices;
        }

        public async Task<>
    }
}