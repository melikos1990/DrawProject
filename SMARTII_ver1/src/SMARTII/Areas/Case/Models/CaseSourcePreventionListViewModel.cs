using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Case.Models
{
    public class CaseSourcePreventionListViewModel
    {
        public CaseSourcePreventionListViewModel()
        {
        }

        public CaseSourcePreventionListViewModel(CaseSource caseSource)
        {
            this.SourceID = caseSource.SourceID;
            this.NodeName = caseSource.CaseSourceUser?.NodeName.DisplayWhenNull();
            this.CreateDateTime = caseSource.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Remark = caseSource.Remark;
        }

        public string SourceID { get; set; }

        public string NodeName { get; set; }

        public string CreateDateTime { get; set; }

        public string Remark { get; set; }
    }
}
