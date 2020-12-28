using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.ViewModel
{
    /// <summary></summary>
    public class ViewMenuTree
    {
        public int id { get; set; }

        /// <summary>名称</summary>
        public string name { get; set; }

        public bool isbtn { get; set; }

        public int order { get; set; }

        public int pid { get; set; }

        public string iconCls { get; set; }
        /// <summary>路径</summary>

        public string path { get; set; }//路径

        /// <summary>模板地址</summary>
        public string component { get; set; }//模板

        /// <summary>是否隐藏</summary>
        public bool? hidden { get; set; } = false;

        /// <summary></summary>
        public meta meta { get; set; } = new meta();

        /// <summary></summary>

        public List<ViewMenuTree> children { get; set; } = new List<ViewMenuTree>();
    }

    public class meta
    {
        /// <summary></summary>
        public string title { get; set; }

        /// <summary></summary>

        public string icon { get; set; }

        /// <summary></summary>
        public bool affix { get; set; } = false;
    }
}