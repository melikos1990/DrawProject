using System.ComponentModel;

namespace SMARTII.Domain.Organization
{
    public enum WorkProcessType
    {
        /// <summary>
        /// 單人模式
        /// </summary>
        [Description("單人模式")]
        Individual = 0,

        /// <summary>
        /// 偕同模式
        /// </summary>
        [Description("偕同模式")]
        Accompanied = 1
    }
}
