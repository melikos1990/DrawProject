using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Organization.Models.VendorNode
{
    public class VendorNodeDetailViewModel
    {
        public VendorNodeDetailViewModel()
        {
        }

        public VendorNodeDetailViewModel(Domain.Organization.VendorNode data)
        {
            this.ID = data.NodeID;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled;
            this.DefindID = data.NodeType;
            this.DefindName = data.OrganizationNodeDefinitaion?.Name;
            this.DefindKey = data.OrganizationNodeDefinitaion?.Key;
            this.CreateUserName = data.CreateUserName;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = data.UpdateUserName;
            this.VendorID = data.VendorParent?.NodeID;
            this.VendorName = data.VendorParent?.Name;
            this.ServingBu = data.HeaderQuarterNodes?.Select(x => x.NodeID).ToList() ?? new List<int>();
            this.Jobs = data.JobPosition?
                            .Select(x => new NodeJobListViewModel(x))
                            .ToList();
        }

        public Domain.Organization.VendorNode ToDomain()
        {
            var data = new Domain.Organization.VendorNode();

            data.NodeID = this.ID;
            data.VendorID = this.VendorID;
            data.Name = this.Name;
            data.NodeType = this.DefindID;
            data.IsEnabled = this.IsEnabled;
            data.HeaderQuarterNodes = this.ServingBu?.Select(buID => new Domain.Organization.HeaderQuarterNode()
            {
                NodeID = buID
            }).ToList() ?? new List<Domain.Organization.HeaderQuarterNode>();

            return data;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int? EnterpriseID { get; set; }
        public int? DefindID { get; set; }
        public string DefindName { get; set; }
        public string DefindKey { get; set; }
        public string CreateDateTime { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateDateTime { get; set; }
        public string UpdateUserName { get; set; }
        public int? VendorID { get; set; }
        public string VendorName { get; set; }
        public List<int> ServingBu { get; set; }

        public List<NodeJobListViewModel> Jobs { get; set; }
    }
}
