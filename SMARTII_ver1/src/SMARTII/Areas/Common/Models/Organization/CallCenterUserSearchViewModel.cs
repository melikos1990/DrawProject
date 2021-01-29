using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.Areas.Common.Models.Organization
{
    public class CallCenterUserSearchViewModel
    {
        public CallCenterUserSearchViewModel()
        {
        }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.USER.IS_ENABLED),
        ExpressionType.Equal)]
        public Boolean? IsEnable { get; set; }

        /// <summary>
        /// 是否為系統使用者
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.USER.IS_SYSTEM_USER),
        ExpressionType.Equal)]
        public Boolean? IsSystemUser { get; set; }

        /// <summary>
        /// 使用者代號
        /// </summary>
        [MSSQLFilter(nameof(Database.SMARTII.USER.USER_ID),
        ExpressionType.Equal)]
        public string UserID { get; set; }

        /// <summary>
        /// 是否撈取全部使用者
        /// </summary>
        public Boolean IsSelf { get; set; }
    }
}