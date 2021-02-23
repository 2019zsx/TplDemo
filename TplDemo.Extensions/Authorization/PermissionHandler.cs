using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TplDemo.Common;
using TplDemo.Common.Message;

namespace TplDemo.Extensions.Authorization
{
    /// <summary></summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        public IAuthenticationSchemeProvider Schemes;

        /// <summary>获取上下文对象</summary>
        private IHttpContextAccessor contextAccessor;

        public PermissionHandler(IHttpContextAccessor _contextAccessor, IAuthenticationSchemeProvider schemes)
        {
            Schemes = schemes;
            // dbbaidu_UserIServices = _baidu_UserIServices;
            contextAccessor = _contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            try
            {
                var httpContext = contextAccessor.HttpContext.Request;

                //
                //是否经过验证
                var isAuthenticated = context.User.Identity.IsAuthenticated;
                if (isAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            catch (Exception ex)
            {
                HttpMsg httpMsg = new HttpMsg();
                httpMsg.HttpContextmsg(StatusCodes.Status200OK, contextAccessor, ex.Message, 30002);
            }
            return Task.CompletedTask;
        }
    }
}