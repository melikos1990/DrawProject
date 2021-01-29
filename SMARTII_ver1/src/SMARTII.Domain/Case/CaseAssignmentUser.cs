using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignmentUser : OrganizationUser
    {
        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派序號
        /// </summary>
        public int AssignmentID { get; set; }

        /// <summary>
        /// 是否為結案單位
        /// </summary>
        public bool IsApply { get; set; }

        /// <summary>
        /// 權責單位/知會單位
        /// </summary>
        public CaseComplainedUserType CaseComplainedUserType { get; set; }

        /// <summary>
        /// 所屬的轉派
        /// </summary>
        public CaseAssignment CaseAssignment { get; set; }
    }
}
