using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Model.DataModel
{
    ///<summary>
    /// 用户信息表
    ///</summary>
    [SugarTable("cxljdzj_yp_software")]
    public partial class YpsoftwareEntity
    {
        /// <summary>
        ///
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "cxljdzj_id")]
        public int id { get; set; }

        /// <summary>
        /// 商铺ID
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_yp_id")]
        public int? yp_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_user")]
        public string user { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_pass")]
        public string pass { get; set; }

        /// <summary>
        /// 条数
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_tiaoshu")]
        public int? tiaoshu { get; set; }

        /// <summary>
        /// 是否允许登录   1正常0关闭
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_tiaoshu")]
        public int? state { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_date")]
        public DateTime? date { get; set; }

        /// <summary>
        ///最后一次登录
        /// </summary>
        [SugarColumn(ColumnName = "cxljdzj_lastdate")]
        public DateTime? lastdate { get; set; }
    }
}