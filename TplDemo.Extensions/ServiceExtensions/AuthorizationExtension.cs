using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Extensions.Authorization;

namespace TplDemo.Extensions.ServiceExtensions
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