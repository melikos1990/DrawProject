using System;

namespace SMARTII.Areas.Master.Models.QuestionClassification
{
    public class QuestionClassificationAnswerViewModel
    {
        /// <summary>
        /// 常見問題代碼
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        /// 常見問題 主旨
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 常見問題 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新人員名稱
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 資料的編輯行為
        /// </summary>
        public AnswerActionType DataType { get; set; } = AnswerActionType.None;
    }

    public enum AnswerActionType
    {
        None = -1,

        Update,

        Add,

        Remove
    }
}