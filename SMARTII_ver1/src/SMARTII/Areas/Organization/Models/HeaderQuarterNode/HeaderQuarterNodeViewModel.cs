using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.HeaderQuarterNode
{
    public class HeaderQuarterNodeViewModel : OrganizationNodeViewModel
    {
        public HeaderQuarterNodeViewModel()
        {
        }

        public HeaderQuarterNodeViewModel(Domain.Organization.HeaderQuarterNode data) : base(data)
        {
            this.EnterpriseID = data.EnterpriseID;
            this.OrganizationType = data.OrganizationType;
            this.BUID = data.BUID;
            this.BUName = data.BusinessParent?.Name;

            this.Children = data.Children?
                                .Select(x => new HeaderQuarterNodeViewModel((Domain.Organization.HeaderQuarterNode)x))
                                .ToList();
        }

        public Domain.Organization.HeaderQuarterNode ToDomain(int? buID = null)
        {
            var result = new Domain.Organization.HeaderQuarterNode();

            int? businessID = this.ConsiderBusinessID(buID);

            result.BUID = businessID;
            result.NodeID = this.ID;
            result.IsEnabled = this.IsEnabled;
            result.Name = this.Name;
            result.EnterpriseID = this.EnterpriseID;
            result.Target = this.Target;

            this.Children?.ForEach(x =>
            {
                result.Children.Add(x.ToDomain(businessID));
            });

            return result;
        }

        public List<HeaderQuarterNodeViewModel> Children { get; set; }

        public int? BUID { get; set; }

        public string BUName { get; set; }

        public int? EnterpriseID { get; set; }

        public int? ConsiderBusinessID(int? buID)
        {
            if (buID.HasValue) return buID;

            if (this.DefindKey == EssentialCache.NodeDefinitionValue.BusinessUnit)
            {
                return this.ID;
            };

            return default(int?);
        }

        public override bool IsPresist
        {
            get
            {
                return DefindKey == EssentialCache.NodeDefinitionValue.BusinessUnit || LeftBoundary == 1 || IsEnabled == false;
            }
        }
    }
}
