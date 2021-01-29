using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Report
{
    /// <summary>
    /// 轉派案件(總部、門市、廠商)
    /// </summary>
    public class CaseAssignmentHeadquartersStore
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
        /// 單號
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 案件來源
        /// </summary>
        public string SourceType { get; set; }
        /// <summary>
        /// 模式
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public string AssignmentType { get; set; }
        /// <summary>
        /// 連絡者型態
        /// </summary>
        public string UnitType { get; set; }
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
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 通知時間
        /// </summary>
        public string NoticeTime { get; set; }
        /// <summary>
        /// 通知內容
        /// </summary>
        public string NoticeContent { get; set; }
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
        /// 分類代號
        /// </summary>
        public int ClassificationID { get; set; }
    }
}
