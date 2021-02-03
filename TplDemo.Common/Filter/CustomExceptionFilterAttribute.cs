using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Filter
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            JsonResult result = null;
            if (exception is BusinessException)
            {
                result = new JsonResult(exception.Message)
                {
                    StatusCode = exception.HResult
                };
            }
            else
            {
                result = new JsonResult("查询出错,请稍后重试")
                {
                    StatusCode = 500
                };
                //result = new JsonResult(exception.Message)
                //{
                //    StatusCode = 500
                //};
                //_logger.LogError(exception, "查询出错,请稍后重试");
                //Agent.Tracer.CurrentTransaction.CaptureException(exception);
            }

            context.Result = result;
        }
    }
}