using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Config
{
    public class Dbconfig
    {
        /// <summary>数据库名称</summary>
        public string dbname { get; set; } = "";

        /// <summary>数控库连接地址</summary>
        public string dburl { get; set; }

        /// <summary>是否关闭</summary>
        public bool isclose { get; set; }

        /// <summary>数据库类型（1 sqlserver 0 mysql）</summary>
        public int dbtype { get; set; }

        public bool ismodel { get; set; }
    }
}