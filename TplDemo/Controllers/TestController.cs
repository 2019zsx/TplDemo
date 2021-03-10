using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Common.Message;
using TplDemo.Model.ViewModel;

namespace TplDemo.Controllers
{
    /// <summary>测试接口</summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public Viewtest GetValidation(Viewtest viewtest)
        {
            return viewtest;
        }
    }
}