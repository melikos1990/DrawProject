using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.MisterDonut
{
    /// <summary>
    /// 來電紀錄/來信紀錄/其他紀錄
    /// </summary>
    public class MisterDonutCallHistory
    {
        /// <summary>
        /// 來源型態
        /// </summary>
        public CaseSourceType CaseSourceType { get; set; }
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
        /// 反應門市
        /// </summary>
        public string ComplainedNodeName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 聯繫電話
        /// </summary>
        public string ConcatMobile { get; set; }
        /// <summary>
        /// 是否開立客訴
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
        /// 結案時間
        /// </summary>
        public DateTime? FinishDateTime { get; set; }
        /// <summary>
        /// 結案日期
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        /// 信件回復日期
        /// </summary>
        public string ReplyDate { get; set; }
        /// <summary>
        /// 信件回復時間
        /// </summary>
        public string ReplyTime { get; set; }

        /// <summary>
        /// 回覆處理內容
        /// </summary>
        public string ReplyContent { get; set; }
    }
}
