using System;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentHistory
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派序號
        /// </summary>
        public int AssignemtID { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 當前轉派狀態
        /// </summary>
        public CaseAssignmentType CaseAssignmentType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }
    }
}