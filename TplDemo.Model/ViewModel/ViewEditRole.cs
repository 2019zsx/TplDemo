﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary></summary>
    public class ViewEditRole
    {
        /// <summary></summary>
        public int id { get; set; }

        /// <summary>角色名称</summary>
        public string roleName { get; set; }

        /// <summary>描述</summary>

        public string describe { get; set; }

        public bool isDeleted { get; set; }
    }
}