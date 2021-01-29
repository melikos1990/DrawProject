using System.ComponentModel;

namespace SMARTII.Domain.Organization
{
    public enum OrganizationType
    {
        /// <summary>
        /// 總部組織
        /// </summary>
        [Description("總部組織")]
        HeaderQuarter,

        /// <summary>
        /// 客服中心
        /// </summary>
        [Description("客服中心")]
        CallCenter,

        /// <summary>
        /// 廠商
        /// </summary>
        [Description("廠商")]
        Vendor
    }
}