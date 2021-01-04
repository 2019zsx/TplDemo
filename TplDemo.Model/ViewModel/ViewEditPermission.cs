using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    public class ViewEditPermission
    {
        public int id { get; set; }

        /// <summary>父ID</summary>
        public int? parentID { get; set; }

        /// <summary>排序</summary>
        public int? orderID { get; set; }

        /// <summary>地址</summary>
        public string path { get; set; }

        /// <summary>图标</summary>
        public string icon { get; set; }

        /// <summary>是否按钮</summary>
        public bool isButton { get; set; }

        /// <summary>标题</summary>
        public string title { get; set; }

        /// <summary>是否启用</summary>
        public bool isEnable { get; set; }

        /// <summary></summary>

        public string component { get; set; }
    }
}