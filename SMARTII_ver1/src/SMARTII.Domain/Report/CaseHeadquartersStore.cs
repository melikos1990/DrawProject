using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    /// <summary>
    /// 轉派案件(總部、門市)
    /// </summary>
    public class CaseHeadquartersStore
    {
        /// <summary>
        /// 企業別
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// 案件來源
        /// </summary>
        public string CaseSource { get; set; }
        /// <summary>
        /// 案件狀態
        /// </summary>
        public string CaseType { get; set; }
        /// <summary>
        /// 案件等級
        /// </summary>
        public string CaseLevel { get; set; }
        /// <summary>
        /// 連絡者型態
        /// </summary>
        public string ContactType { get; set; }
        /// <summary>
        /// 連絡者
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 被反應者
        /// </summary>
        public string Respondent { get; set; }
        /// <summary>
        /// 組織
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 期望期限
        /// </summary>
        public string ExpectedPeriod { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 是否開單
        /// </summary>
        public string OpenInvoicet { get; set; }
        /// <summary>
        /// 通知對象
        /// </summary>
        public string NoticePreson { get; set; }
        /// <summary>
        /// 通知時間
        /// </summary>
        public string NoticeTime { get; set; }
        /// <summary>
        /// 通知內容
        /// </summary>
        public string NoticeContent { get; set; }
        /// <summary>
        /// 回覆時間
        /// </summary>
        public string RetryTime { get; set; }
        /// <summary>
        /// 回覆內容
        /// </summary>
        public string RetryContent { get; set; }
        /// <summary>
        /// 銷案時間
        /// </summary>
        public string CloseCaseTime { get; set; }
        /// <summary>
        /// 銷案內容
        /// </summary>
        public string CloseCaseContent { get; set; }
        /// <summary>
        /// 應完成時間
        /// </summary>
        public string DueTime { get; set; }
        /// <summary>
        /// 結案時間
        /// </summary>
        public string FinishTime { get; set; }
        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        public string Preson { get; set; }
        /// <summary>
        /// 未結案轉派
        /// </summary>
        public string UnfinishedAssignmentCase { get; set; }
        /// <summary>
        /// 分類ID
        /// </summary>
        public int? ClassificationID { get; set; }
    }
}
