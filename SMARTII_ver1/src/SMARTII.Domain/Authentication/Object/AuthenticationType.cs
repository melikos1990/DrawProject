using System;
using System.ComponentModel;

namespace SMARTII.Domain.Authentication.Object
{
    [Flags]
    public enum AuthenticationType
    {
        /// <summary>
        /// 無
        /// </summary>
        [Description("無")]
        None = 0,

        /// <summary>
        /// 查詢
        /// </summary>
        [Description("查詢")]
        Read = 1,

        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Add = 2,

        /// <summary>
        /// 刪除
        /// </summary>
        [Description("刪除")]
        Delete = 4,

        /// <summary>
        /// 更新
        /// </summary>
        [Description("更新")]
        Update = 8,

        /// <summary>
        /// 報表控制
        /// </summary>
        [Description("報表控制")]
        Report = 16,

        /// <summary>
        /// Admin
        /// </summary>
        [Description("Admin")]
        Admin = 32
    }
}