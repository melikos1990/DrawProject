using System.ComponentModel;

namespace SMARTII.Domain.Master
{
    public enum CaseAssignGroupType
    {
        /// <summary>
        /// 派工群組
        /// </summary>
        [Description("派工群組")]
        Normal = 0,

        /// <summary>
        /// 大量叫修
        /// </summary>
        [Description("大量叫修")]
        PPCRepair = 1
    }
}