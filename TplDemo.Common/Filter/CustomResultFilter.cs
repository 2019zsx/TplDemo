using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TplDemo.Common.Message;

namespace TplDemo.Common.Filter
{
    /// <summary></summary>
    public class CustomResultFilter : ActionFilterAttribute
    {
        /// <summary></summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new PageModel<object>();
                result.state = 30002;
                result.msg = context.ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                context.Result = new JsonResult(result);
            }
            else
            {
                base.OnResultExecuting(context);
            }
        }
    }
}