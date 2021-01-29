using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.VendorNode
{
    public class VendorNodeViewModel : OrganizationNodeViewModel
    {
        public VendorNodeViewModel()
        {
        }

        public VendorNodeViewModel(Domain.Organization.VendorNode data) : base(data)
        {
            this.OrganizationType = data.OrganizationType;
            this.VendorID = data.VendorID;
            this.VendorName = data.VendorParent?.Name;
            this.Children = data.Children?
                                .Select(x => new VendorNodeViewModel((Domain.Organization.VendorNode)x))
                                .ToList();
        }

        public Domain.Organization.VendorNode ToDomain(int? vendorID = null)
        {
            var result = new Domain.Organization.VendorNode();

            int? _vendorID = this.ConsiderVendorID(vendorID);

            result.NodeID = this.ID;
            result.IsEnabled = this.IsEnabled;
            result.Name = this.Name;
            result.Target = this.Target;
            result.VendorID = _vendorID;

            this.Children?.ForEach(x =>
            {
                result.Children.Add(x.ToDomain(_vendorID));
            });

            return result;
        }

        public List<VendorNodeViewModel> Children { get; set; }

        public string VendorName { get; set; }

        public int? VendorID { get; set; }


        public int? ConsiderVendorID(int? vendorID)
        {
            if (vendorID.HasValue) return vendorID;

            if (this.DefindKey == EssentialCache.NodeDefinitionValue.Vendor) return this.ID;

            return default(int?);
        }
    }
}
