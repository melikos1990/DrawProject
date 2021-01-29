using System.ComponentModel;

namespace SMARTII.Domain.Master
{
    public enum WorkType
    {
        /// <summary>
        /// 上班日
        /// </summary>
        [Description("工作日")]
        WorkOn = 0,

        /// <summary>
        /// 休假日
        /// </summary>
        [Description("休假日")]
        WorkOff = 1
    }
}
