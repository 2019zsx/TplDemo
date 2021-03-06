﻿using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("Permission")]
    public partial class Permission
    {
        /// <summary></summary>
        [SugarColumn(IsIgnore = false, IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>名称</summary>
        public string Name { get; set; }

        /// <summary>父ID</summary>
        public int? ParentID { get; set; }

        /// <summary>排序</summary>
        public int? orderID { get; set; }

        /// <summary>地址</summary>
        public string Path { get; set; }

        /// <summary>图标</summary>
        public string Icon { get; set; }

        /// <summary>是否按钮</summary>
        public bool IsButton { get; set; }

        /// <summary>标题</summary>
        public string Title { get; set; }

        /// <summary>是否启用</summary>
        public bool isEnable { get; set; }

        /// <summary>模板</summary>
        public string component { get; set; }
    }
}