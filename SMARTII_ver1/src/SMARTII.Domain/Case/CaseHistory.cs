using System;

namespace SMARTII.Domain.Case
{
    public class CaseHistory
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
        /// 異動內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 案件狀態
        /// </summary>
        public CaseType CaseType { get; set; }

        /// <summary>
        /// 立案時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 立案時間
        /// </summary>
        public string CreateUserName { get; set; }
    }
}