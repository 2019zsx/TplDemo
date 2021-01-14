using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.IServices;

namespace TplDemo.Controllers
{
    /// <summary></summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolepermissionController : ControllerBase
    {
        private RolepermissionIServices rolepermissionIServices;

        /// <summary></summary>
        /// <param name="_rolepermissionIServices"></param>
        public RolepermissionController(RolepermissionIServices _rolepermissionIServices)
        {
            rolepermissionIServices = _rolepermissionIServices;
        }
    }
}