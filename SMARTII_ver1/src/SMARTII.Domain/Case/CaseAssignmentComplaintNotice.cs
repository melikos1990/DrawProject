using System.Collections.Generic;
using System.ComponentModel;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentComplaintNotice : CaseAssignmentBase
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [Description("流水號")]
        public int ID { get; set; }

        /// <summary>
        /// 通知對象
        /// </summary>
        [Description("通知對象")]
        public List<CaseAssignmentComplaintNoticeUser> Users { get; set; }

        /// <summary>
        /// 通知狀態
        /// ※目前就業為行為來說 , 通知發出就是已通知
        /// 因此並未針對該欄位進行寫入DB
        /// </summary>
        [Description("通知狀態")]
        public CaseAssignmentComplaintNoticeType CaseAssignmentComplaintNoticeType { get; set; } = CaseAssignmentComplaintNoticeType.Noticed;

        #region override

        public override CaseAssignmentProcessType CaseAssignmentProcessType { get; set; } =
                        CaseAssignmentProcessType.Notice;

        #endregion
    }
}
