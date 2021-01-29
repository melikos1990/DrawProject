using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentComplaintNoticeUser : ConcatableUser
    {
        /// <summary>
        /// 識別編號
        /// </summary>
        [Description("識別編號")]

        public int ID { get; set; }

        /// <summary>
        /// 通知序號
        /// </summary>
        [Description("通知序號")]
        public int NoticeID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        [Description("案件代號")]

        public string CaseID { get; set; }

        /// <summary>
        /// 所屬的通知
        /// </summary>
        public CaseAssignmentComplaintNotice Notice { get; set; }
    }
}
