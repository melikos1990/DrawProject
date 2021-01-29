using System.Linq;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseNearlyListViewModel
    {
        public CaseNearlyListViewModel()
        {
        }

        public CaseNearlyListViewModel(Domain.Case.Case @case)
        {
            this.CaseID = @case.CaseID;
            this.CreateDateTime = @case.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Content = @case.Content;
            this.CaseType = @case.CaseType.GetDescription();


            var caseComplainedUser = @case.CaseComplainedUsers?
                                    .Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility)
                                    .OrderBy(x=>x.ID).FirstOrDefault();

            this.NodeName = "";

            if (caseComplainedUser != null)
            {
                if (caseComplainedUser.UnitType == UnitType.Organization)
                {
                    this.NodeName = $"{caseComplainedUser.BUName}-{caseComplainedUser.NodeName}";
                }
                else if (caseComplainedUser.UnitType == UnitType.Store)
                {
                    this.NodeName = $"{caseComplainedUser.StoreNo}-{caseComplainedUser.NodeName}";
                }
            }
            
        }

        public string CaseID { get; set; }

        public string CreateDateTime { get; set; }

        public string Content { get; set; }

        public string CaseType { get; set; }

        public string NodeName { get; set; }
    }
}
