using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public enum CaseComplainedUserType
    {
        /// <summary>
        /// 知會單位
        /// </summary>
        [Description("知會單位")]
        Notice,

        /// <summary>
        /// 權責單位
        /// </summary>
        [Description("權責單位")]
        Responsibility
    }
}