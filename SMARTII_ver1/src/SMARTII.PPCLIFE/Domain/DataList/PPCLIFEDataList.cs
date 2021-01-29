using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.COMMON_BU;

namespace SMARTII.PPCLIFE.Domain.DataList
{
    /// <summary>
    /// 統一來電紀錄、代理品牌、自有品牌、醫學美容
    /// </summary>
    public class PPCLIFEDataList
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
        public List<PPCLIFECallHistory> OnCallHistory { get; set; }
        /// <summary>
        /// 來信紀錄
        /// </summary>
        public List<PPCLIFECallHistory> OnEmailHistory { get; set; }
        /// <summary>
        /// 其他紀錄
        /// </summary>
        public List<PPCLIFECallHistory> OtherHistory { get; set; }
        /// <summary>
        /// 客訴紀錄-一般客訴
        /// </summary>
        public List<PPCLIFEComplaintHistory> OnGeneralComplaint { get; set; }
        /// <summary>
        /// 客訴紀錄-一般客訴(上個月未結案)
        /// </summary>
        public List<PPCLIFEComplaintHistory> OnGeneralComplaintNotFinished { get; set; }
        /// <summary>
        /// 客訴紀錄-重大客訴
        /// </summary>
        public List<PPCLIFEComplaintHistory> OnUrgentComplaint { get; set; }
        /// <summary>
        /// 客訴紀錄-重大客訴(上個月未結案)
        /// </summary>
        public List<PPCLIFEComplaintHistory> OnUrgentComplaintNotFinished { get; set; }

    }
}
