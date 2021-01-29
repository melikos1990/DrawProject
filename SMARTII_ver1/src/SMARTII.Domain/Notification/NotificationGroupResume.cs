using System;

namespace SMARTII.Domain.Notification
{
    public class NotificationGroupResume
    {
        public NotificationGroupResume()
        {
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 通知群組代號
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 企業代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 企業名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 規則
        /// </summary>
        public string[] Target { get; set; }

        /// <summary>
        /// 發送內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 發送結果
        /// </summary>
        public NotificationGroupResultType NotificationGroupResultType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string EMLFilePath { get; set; }

        /// <summary>
        /// 所屬提醒群組
        /// </summary>
        public NotificationGroup NotificationGroup { get; set; }
    }
}
