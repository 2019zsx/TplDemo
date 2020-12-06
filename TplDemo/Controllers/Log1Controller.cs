using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TplDemo.Controllers
{
    /// <summary>
    ///  添加日志实例
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Log1Controller : ControllerBase
    {
        private readonly ILogger<Log1Controller> logger;

        /// <summary>
        ///
        /// </summary>
        public Log1Controller(ILogger<Log1Controller> _logger)
        {
            logger = _logger;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Log()
        {
            logger.LogError("错误日志");
            logger.LogDebug("Debug日志");
            logger.LogWarning("Warning日志");
            return Ok("添加日志成功");
        }
    }
}