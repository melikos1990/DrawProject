using SMARTII.Domain.Case;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentCommunicateViewModel : CaseAssignmentBaseViewModel
    {
        public CaseAssignmentCommunicateViewModel()
        {
        }

        public CaseAssignmentCommunicateViewModel(CaseAssignmentCommunicate communicate)
        {
            this.CaseID = communicate.CaseID;
            this.Content = communicate.Content;
            this.CreateDateTime = communicate.CreateDateTime;
            this.CreateUserName = communicate.CreateUserName;
            this.EMLFilePath = communicate.EMLFilePath;
            this.ID = communicate.ID;
            this.NodeID = communicate.NodeID;
            this.OrganizationType = communicate.OrganizationType;
        }

        public CaseAssignmentCommunicate ToDomain()
        {
            var result = new CaseAssignmentCommunicate();

            result.CaseID = this.CaseID;
            result.Content = this.Content;
            result.CreateDateTime = this.CreateDateTime;
            result.CreateUserName = this.CreateUserName;
            result.EMLFilePath = this.EMLFilePath;
            result.ID = this.ID;
            result.NodeID = this.NodeID;
            result.OrganizationType = this.OrganizationType;
            result.NotificationDateTime = this.NotificationDateTime;

            return result;
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }
    }
}
