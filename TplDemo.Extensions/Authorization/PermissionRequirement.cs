using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Extensions.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(params string[] codes)
        {
            this.Codes = codes;
        }

        public string[] Codes { get; private set; }
    }
}