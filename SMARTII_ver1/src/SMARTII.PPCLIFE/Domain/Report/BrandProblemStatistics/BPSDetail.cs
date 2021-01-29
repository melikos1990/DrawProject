using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.PPCLIFE.Domain
{
    /// <summary>
    /// 品牌商品與問題歸類-數據統計報表 (明細)
    /// </summary>
    public class BPSDetail
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 時間
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 問題分類
        /// </summary>
        public QuestionClassification ClassList { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string ComplainedNodeName { get; set; }
        /// <summary>
        /// 反應門市
        /// </summary>
        public string Store { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 聯繫電話
        /// </summary>
        public string ConcatMobile { get; set; }
        /// <summary>
        /// 是否開單
        /// </summary>
        public string IsInvoice { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 回覆
        /// </summary>
        public string FinishContent { get; set; }
        /// <summary>
        /// 處理人員
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 轉派單位人員
        /// </summary>
        public string AssignmentUserName { get; set; }
        /// <summary>
        /// 完成時間
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        /// 回覆處理內容
        /// </summary>
        public string ReplyContent { get; set; }

        /// <summary>
        /// 回覆顧客方式清單
        /// </summary>
        public List<CaseFinishReasonData> ReplyCustomerList { get; set; }

        /// <summary>
        /// 問題要因
        /// </summary>
        public List<CaseFinishReasonData> CauseProblemList { get; set; }

    }
}
