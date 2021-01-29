using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Master;

namespace SMARTII.COMMON_BU
{
    /// <summary>
    /// 通路紀錄
    /// 21世紀和酷聖石
    /// </summary>
    public class CommonPathwayHistory
    {
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
        /// 完成回覆日期
        /// </summary>
        public string FinishDate { get; set; }
        /// <summary>
        /// 完成回覆時間
        /// </summary>
        public string FinishTime { get; set; }
        /// <summary>
        /// 處理人員
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 轉派單位人員
        /// </summary>
        public string AssignmentUserName { get; set; }
    }
}
