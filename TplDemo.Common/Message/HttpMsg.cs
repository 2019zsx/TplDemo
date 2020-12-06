using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Common.Helper;

namespace TplDemo.Common.Message
{
    public class HttpMsg
    { /// <summary>
      ///
      /// </summary>
      /// <param name="msg">消息</param>
      /// <param name="state">状态码</param>
        public void HttpContextmsg(int Status, IHttpContextAccessor contextAccessor, string msg, int state)
        {
            var httpContext = contextAccessor.HttpContext;
            //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
            var payload = JsonData.Serializer(new PageModel<object>() { msg = msg, state = state });
            //自定义返回的数据类型
            httpContext.Response.ContentType = "application/json";
            //自定义返回状态码，默认为401 我这里改成 200
            httpContext.Response.StatusCode = Status;
            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //输出Json数据结果
            httpContext.Response.WriteAsync(payload);
        }
    }
}