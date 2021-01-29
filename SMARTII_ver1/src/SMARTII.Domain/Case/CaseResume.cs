using System;
using System.ComponentModel;
using SMARTII.Domain.Common;

namespace SMARTII.Domain.Case
{
    public class CaseResume
    {
        public CaseResume()
        {
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        [Description("案件編號")]
        public string CaseID { get; set; }

        /// <summary>
        /// 案件型態
        /// </summary>
        [Description("案件型態")]
        public CaseType CaseType { get; set; }

        /// <summary>
        /// 異動內容
        /// </summary>
        [Description("異動內容")]
        public string Content { get; set; }

        /// <summary>
        /// 立案時間
        /// </summary>
        [Description("立案時間")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 立案人員
        /// </summary>
        [Description("立案人員")]
        public string CreateUserName { get; set; }
    }
}
