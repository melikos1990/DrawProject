using System;

namespace SMARTII.Domain.Case
{
    public class CaseSourceHistory
    {
        public CaseSourceHistory()
        {
        }

        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 來源代號
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        public CaseSource CaseSource { get; set; }
    }
}