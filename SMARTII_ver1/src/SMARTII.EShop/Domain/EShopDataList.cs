using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.COMMON_BU;

namespace SMARTII.EShop
{
    public class EShopDataList
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
        public List<EShopCallHistory> OnCallHistory { get; set; }
        /// <summary>
        /// 來信紀錄
        /// </summary>
        public List<EShopCallHistory> OnEmailHistory { get; set; }
        /// <summary>
        /// 其他紀錄
        /// </summary>
        public List<EShopCallHistory> OnOtherHistory { get; set; }
    }
}
