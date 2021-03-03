using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Helper
{
    /// <summary>获取当前IP</summary>
    public class CheckIP
    {
        public static HttpContextAccessor context;

        public CheckIP(HttpContextAccessor _context)
        {
            context = _context;
        }

        /// <summary>获取客户端IP地址</summary>
        /// <returns></returns>
        public static string GetIP()
        {
            return context.HttpContext?.Connection.RemoteIpAddress.ToString();
        }
    }
}