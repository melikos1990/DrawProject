using System.ComponentModel;

namespace SMARTII.Domain.Authentication.Object
{
    public enum UserType
    {
        /// <summary>
        /// 系統使用者
        /// </summary>
        [Description("系統使用者")]
        System,

        /// <summary>
        /// 公司內部人員
        /// </summary>
        [Description("公司內部人員")]
        AD
    }
}