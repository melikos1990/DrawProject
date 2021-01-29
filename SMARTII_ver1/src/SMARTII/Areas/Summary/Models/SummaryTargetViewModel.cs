using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Summary.Models
{
    public class SummaryTargetViewModel
    {
        public SummaryTargetViewModel(IOrganizationNode node)
        {
            this.TargetName = node.Name;
            this.TargetID = node.NodeID;
        }

        public string TargetName { get; set; }

        public int TargetID { get; set; }
    }
}