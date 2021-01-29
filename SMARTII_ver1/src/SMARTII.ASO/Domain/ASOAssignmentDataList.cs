using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.COMMON_BU;

namespace SMARTII.ASO.Domain
{
    public class ASOAssignmentDataList
    {
        /// <summary>
        /// 彙總紀錄
        /// </summary>
        public SummaryInfoData SummaryInformation { get; set; }
        /// <summary>
        /// 本日彙總表
        /// </summary>
        public SummaryInfoData DailySummary { get; set; }
        /// <summary>
        /// 本月彙總表
        /// </summary>
        public SummaryInfoData MonthSummary { get; set; }
        /// <summary>
        /// 來電紀錄
        /// </summary>
        public List<ASOAssignmentCallHistory> OnCallHistory { get; set; }
        /// <summary>
        /// 來信紀錄
        /// </summary>
        public List<ASOAssignmentCallHistory> OnEmailHistory { get; set; }
        /// <summary>
        /// 其他紀錄
        /// </summary>
        public List<ASOAssignmentCallHistory> OnOtherHistory { get; set; }
        /// <summary>
        /// 門市紀錄
        /// </summary>
        public List<ASOAssignmentCallHistory> OnStoreHistory { get; set; }
    }
}
