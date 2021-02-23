using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TplDemo.Common.Message;

namespace TplDemo.Common.Filter
{
    /// <summary>实体特性使用</summary>
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
                var ModelState = context.ModelState.Values.Where(c => c.ValidationState != ModelValidationState.Valid).FirstOrDefault().Errors.FirstOrDefault();
                if (ModelState != null)
                {
                    result.msg = ModelState.ErrorMessage;
                }

                context.Result = new JsonResult(result);
            }
            else
            {
                base.OnResultExecuting(context);
            }
        }
    }
}