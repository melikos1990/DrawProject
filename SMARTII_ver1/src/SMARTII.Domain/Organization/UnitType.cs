using System.ComponentModel;

namespace SMARTII.Domain.Organization
{
    public enum UnitType
    {
        /// <summary>
        /// 消費者
        /// </summary>
        [Description("消費者")]
        Customer,

        /// <summary>
        /// 門市
        /// </summary>
        [Description("門市")]
        Store,

        /// <summary>
        /// 組織
        /// </summary>
        [Description("組織")]
        Organization,
    }
}
