using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace TplDemo.Model.ViewModel
{
    public class Viewtest
    {
        [Required(ErrorMessage = "请填写消息")]
        public string i { get; set; }
    }
}