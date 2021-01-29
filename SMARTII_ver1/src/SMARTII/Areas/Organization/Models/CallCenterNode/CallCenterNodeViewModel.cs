using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.CallCenterNode
{
    public class CallCenterNodeViewModel : OrganizationNodeViewModel
    {
        public CallCenterNodeViewModel()
        {
        }

        public CallCenterNodeViewModel(Domain.Organization.CallCenterNode data) : base(data)
        {
            this.OrganizationType = data.OrganizationType;
            this.CallCenterID = data.CallCenterID;
            this.CallCenterName = data.CallCenterParent?.Name;
            this.Children = data.Children?
                                .Select(x => new CallCenterNodeViewModel((Domain.Organization.CallCenterNode)x))
                                .ToList();
        }

        public Domain.Organization.CallCenterNode ToDomain(int? callCenterID = null)
        {
            var result = new Domain.Organization.CallCenterNode();

            int? _callCenterID = this.ConsiderCallCenterID(callCenterID);

            result.NodeID = this.ID;
            result.IsEnabled = this.IsEnabled;
            result.Name = this.Name;
            result.Target = this.Target;
            result.CallCenterID = _callCenterID;

            this.Children?.ForEach(x =>
            {
                result.Children.Add(x.ToDomain(_callCenterID));
            });

            return result;
        }

        public List<CallCenterNodeViewModel> Children { get; set; }

        public int? CallCenterID { get; set; }

        public string CallCenterName { get; set; }

        public int? ConsiderCallCenterID(int? callCenterID)
        {
            if (callCenterID.HasValue) return callCenterID;

            if (this.DefindKey == EssentialCache.NodeDefinitionValue.CallCenter) return this.ID;

            return default(int?);
        }
    }
}
