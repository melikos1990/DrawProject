using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    /// <summary>
    /// 反應者
    /// </summary>
    public class CaseSourceUser : ConcatableUser
    {
        public CaseSourceUser()
        {
        }

        /// <summary>
        /// 來源編號
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 所屬案件來源
        /// </summary>
        public CaseSource CaseSource { get; set; }
    }
}