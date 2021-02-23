﻿using Microsoft.AspNetCore.Authentication;
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

        /// <summary></summary>
        //private Baidu_userIServices dbbaidu_UserIServices;

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

        #region MyRegion

        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        //protected override async Task HandleRequirementAsync(
        //  AuthorizationHandlerContext context,
        //    PermissionRequirement requirement)
        //{
        //    //AuthorizationFilterContext filterContext = context.Resource as AuthorizationFilterContext;
        //    //HttpContext httpContext = filterContext.HttpContext;
        //    //AuthenticateResult result = await httpContext.AuthenticateAsync(Schemes.GetDefaultAuthenticateSchemeAsync().Result.Name);
        //    // 先判断Id是否存在
        //    int uid = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value.ObjToInt();
        //    var isAuthenticated = context.User.Identity.IsAuthenticated;
        //    if (isAuthenticated)
        //    {
        //        if (uid == 0)
        //        {
        //            context.Fail();
        //        }
        //        else
        //        {
        //            var userdata = await dbbaidu_UserIServices.Query(c => c.Id == uid && c.Block == false);
        //            if (userdata.Count() == 0)
        //            {
        //                context.Fail();
        //            }
        //            context.Succeed(requirement);
        //        }
        //    }

        //    //if (result.Succeeded)
        //    //{
        //    //    if (uid == 0)
        //    //    {
        //    //        context.Fail();
        //    //    }
        //    //    else
        //    //    {
        //    //        var userdata = await dbbaidu_UserIServices.Query(c => c.Id == uid && c.Block == true);
        //    //        if (userdata.Count() == 0)
        //    //        {
        //    //            context.Fail();
        //    //        }
        //    //        context.Succeed(requirement);
        //    //    }
        //    //}
        //}

        #endregion MyRegion
    }
}