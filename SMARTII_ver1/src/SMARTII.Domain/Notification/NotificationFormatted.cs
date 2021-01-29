namespace SMARTII.Domain.Notification
{
    public static class NotificationFormatted
    {
        /// <summary>
        /// 通知群組設定
        /// {0} : 企業別
        /// {1} : 標的
        /// {2} : 群組名稱
        /// </summary>
        public static string NotificationGroup = "{0} {1} 已達到示警次數 \n {2}";

        /// <summary>
        /// 職代分配
        /// {0} : 移轉人
        /// {1} : 移轉筆數
        /// </summary>
        public static string CaseAssign = "{0} 已移轉 {1} 筆案件給您";

        /// <summary>
        /// 案件銷案
        /// {0} : 企業別
        /// {1} : 轉派對象
        /// {2} : 通知內容
        /// </summary>
        public static string CaseFinished = "{0} {1} 已銷案 \n {2}";

        /// <summary>
        /// 案件指派
        /// {0} : 指派人
        /// {1} : 轉移數
        /// </summary>
        public static string CaseAdopt = "{0} 已指派 {1} 筆官網來信案件給您";

        /// <summary>
        /// 案件異動
        /// {0} : 異動人
        /// {1} : 異動內容
        /// </summary>
        public static string CaseModify = "{0} 已異動您的案件 \n {1}";

        /// <summary>
        /// 官網來信提醒
        /// {0} : 企業別
        /// {1} : 信件內容
        /// </summary>
        public static string MailIncoming = "{0} 官網來信通知 \n {1}";

        /// <summary>
        /// 佈告欄
        /// {0} : 標題
        /// {1} : 內容
        /// </summary>
        public static string Billboard = "{0} \n {1}";
        
        /// <summary>
        /// 案件提醒機制
        /// {0} : 內容
        /// </summary>
        public static string CaseRemind = "{0}";
    }
}
