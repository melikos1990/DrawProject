﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.COMMON_BU;

namespace SMARTII.ICC.Domain
{
    public class InComm_DataList
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
        public List<InComm_CallHistory> OnCallHistory { get; set; }
        /// <summary>
        /// 來信紀錄
        /// </summary>
        public List<InComm_CallHistory> OnEmailHistory { get; set; }
        /// <summary>
        /// 其他紀錄
        /// </summary>
        public List<InComm_CallHistory> OnOtherHistory { get; set; }
    }
}
