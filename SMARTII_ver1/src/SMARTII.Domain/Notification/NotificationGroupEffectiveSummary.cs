using System;

namespace SMARTII.Domain.Notification
{
    public class NotificationGroupEffectiveSummary
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 提醒群組名稱代號
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 預計達標數
        /// </summary>
        public int ExpectArriveCount { get; set; }

        /// <summary>
        /// 實際達標數
        /// </summary>
        public int ActualArriveCount { get; set; }

        /// <summary>
        /// 案件總數
        /// </summary>
        public string[] CaseIDs { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 是否通知過
        /// </summary>
        public bool IsProcess { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

    }
}
