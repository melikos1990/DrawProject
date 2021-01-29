using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.COMMON_BU;

namespace SMARTII._21Century.Domain
{
    public class _21CenturyDataList
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
        public List<CommonCallHistory> CommonOnCallsHistory { get; set; }
        /// <summary>
        /// 來信紀錄
        /// </summary>
        public List<CommonCallHistory> CommonOnEmailHistory { get; set; }
        /// <summary>
        /// 其他紀錄
        /// </summary>
        public List<CommonCallHistory> CommonOthersHistory { get; set; }
        /// <summary>
        /// 通路紀錄
        /// </summary>
        public List<CommonPathwayHistory> CommonPathwayHistory { get; set; }
        /// <summary>
        /// 客訴紀錄
        /// </summary>
        public List<_21CenturyComplaintHistory> _21CenturyComplaintHistory { get; set; }
    }
}
