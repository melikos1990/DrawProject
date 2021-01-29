using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Master;

namespace SMARTII.ColdStone
{
    /// <summary>
    /// 客訴紀錄
    /// </summary>
    public class ColdStoneComplaintHistory
    {
        /// <summary>
        /// 序號
        /// </summary>
        public string InvoiceID { get; set; }
        /// <summary>
        /// 來源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 時間
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 區
        /// </summary>
        public string Zone { get; set; }
        /// <summary>
        /// 客訴門市
        /// </summary>
        public string ComplainedNodeName { get; set; }
        /// <summary>
        /// 客訴類別 問題分類
        /// </summary>
        public QuestionClassification ClassList { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 聯繫電話
        /// </summary>
        public string ConcatMobile { get; set; }
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
        /// 提供日期
        /// </summary>
        public string InvoiceCreateDate { get; set; }
    }
}
