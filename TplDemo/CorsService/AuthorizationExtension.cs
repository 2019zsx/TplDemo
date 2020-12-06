using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TplDemo.Extensions.Authorization;

//using TplDemo.Common.Authorization;

namespace TplDemo.CorsService

{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin",
                  policy => policy
                    .Requirements
                    .Add(new PermissionRequirement("user")));
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            return services;
        }
    }
}