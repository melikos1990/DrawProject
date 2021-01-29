using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.OpenPoint
{
    /// <summary>
    /// 來電紀錄/來信紀錄
    /// </summary>
    public class OpenPointCallHistory
    {
        /// <summary>
        /// 來源型態
        /// </summary>
        public CaseSourceType CaseSourceType { get; set; }
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
        /// 反應門市
        /// </summary>
        public string ComplainedNodeName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// E-Mail
        /// </summary>
        public string EMail { get; set; }
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
        /// 回覆時間
        /// </summary>
        public string FinishReplyDateTime { get; set; }
        /// <summary>
        /// 結案時間
        /// </summary>
        public string FinishDateTime { get; set; }
        /// <summary>
        /// 轉派單位人員
        /// </summary>
        public string AssignmentUserName { get; set; }
        /// <summary>
        /// 問題分類
        /// </summary>
        public QuestionClassification ClassList { get; set; }
        /// <summary>
        /// 反應日期
        /// </summary>
        public string IncomingDateTime { get; set; }
        
    }
}
