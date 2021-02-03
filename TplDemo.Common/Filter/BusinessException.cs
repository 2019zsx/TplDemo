using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Filter
{
    public class BusinessException : Exception
    {
        public BusinessException(int hResult, string message)
            : base(message)
        {
            base.HResult = hResult;
        }
    }
}