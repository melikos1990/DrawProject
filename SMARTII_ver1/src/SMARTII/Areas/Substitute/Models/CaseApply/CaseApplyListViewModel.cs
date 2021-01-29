using SMARTII.Domain.Case;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Substitute.Models.CaseApply
{
    public class CaseApplyListViewModel
    {
        public CaseApplyListViewModel(Domain.Case.Case Case)
        {
            this.ApplyUserID = Case.ApplyUserID;
            this.ApplyUserName = Case.ApplyUserName;
            this.CaseID = Case.CaseID;
            this.CaseType = Case.CaseType.GetDescription();
            this.CaseWarningName = Case.CaseWarning.Name;
            this.CreateDateTime = Case.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.NodeID = Case.NodeID;
            this.NodeName = Case.NodeName;
            this.SourceID = Case.SourceID;
        }

        public string ApplyUserID { get; set; }
        public string ApplyUserName { get; set; }
        public string CaseID { get; set; }
        public string CaseType { get; set; }
        public string CaseWarningName { get; set; }
        public string CreateDateTime { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public string SourceID { get; set; }
    }
}
