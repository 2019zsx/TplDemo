using System;
using System.Collections.Generic;
using System.Text;
using TplDemo.Common.Validation;

namespace TplDemo.Model.ViewModel
{
    public class Viewtest
    {
        [ClassicMovie(100)]
        public int i { get; set; }
    }
}