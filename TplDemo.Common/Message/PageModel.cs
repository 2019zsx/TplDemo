using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Common.Message
{
    public class PageModel<T>
    {
        public int code { get; set; } = 0;

        /// <summary>
        /// 返回编码
        /// </summary>
        public int state { get; set; } = 10001;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "操作成功";

        /// <summary>
        /// 数据总数
        /// </summary>
        public long count { get; set; } = 0;

        /// <summary>
        /// 返回数据
        /// </summary>
        public T data { get; set; }

        public bool success { get; set; } = true;
    }
}