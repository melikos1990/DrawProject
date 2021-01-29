using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentResumeViewModel
    {
        public CaseAssignmentResumeViewModel()
        {
        }

        public CaseAssignmentResumeViewModel(CaseAssignmentResume resume)
        {
            this.CaseAssignmentID = resume.CaseAssignmentID;
            this.CaseAssignmentType = resume.CaseAssignmentType;
            this.CaseAssignmentTypeName = resume.CaseAssignmentType.GetDescription();
            this.CaseID = resume.CaseID;
            this.Content = resume.Content;
            this.CreateDateTime = resume.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateNodeID = resume.CreateNodeID;
            this.CreateNodeName = resume.CreateNodeName;
            this.CreateOrganizationType = resume.CreateOrganizationType;
            this.CreateOrganizationTypeName = resume.CreateOrganizationType?.GetDescription();
            this.CreateUserName = resume.CreateUserName;
            this.IsReply = resume.IsReply;
         
        }

        public int Index { get; set; }

        public string CaseID { get; set; }

        public int CaseAssignmentID { get; set; }

        public string Content { get; set; }
        public CaseAssignmentType CaseAssignmentType { get; set; }

        public string CaseAssignmentTypeName { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public int? CreateNodeID { get; set; }

        public string CreateNodeName { get; set; }

        public OrganizationType? CreateOrganizationType { get; set; }

        public string CreateOrganizationTypeName { get; set; }

        public bool IsReply { get; set; }
    }
}
