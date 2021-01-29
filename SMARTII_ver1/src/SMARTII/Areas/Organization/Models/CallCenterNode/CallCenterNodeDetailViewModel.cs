using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.CallCenterNode
{
    public class CallCenterNodeDetailViewModel
    {
        public CallCenterNodeDetailViewModel()
        {
        }

        public CallCenterNodeDetailViewModel(Domain.Organization.CallCenterNode data)
        {
            this.ID = data.NodeID;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled;
            this.IsWorkProcessNotice = data.IsWorkProcessNotice;
            this.WorkProcessType = data.WorkProcessType;
            this.DefindID = data.OrganizationNodeDefinitaion?.ID;
            this.DefindName = data.OrganizationNodeDefinitaion?.Name;
            this.DefindKey = data.OrganizationNodeDefinitaion?.Key;
            this.CreateUserName = data.CreateUserName;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = data.UpdateUserName;
            this.CallCenterID = data.CallCenterParent?.NodeID;
            this.CallCenterName = data.CallCenterParent?.Name;
            this.ServingBu = data.HeaderQuarterNodes?.Select(x => x.NodeID).ToList() ?? new List<int>();
            this.Jobs = data.JobPosition?
                            .Select(x => new NodeJobListViewModel(x)).OrderBy(x=>x.Level)
                            .ToList();
        }

        public Domain.Organization.CallCenterNode ToDomain()
        {
            var data = new Domain.Organization.CallCenterNode();

            data.NodeID = this.ID;
            data.Name = this.Name;
            data.NodeType = this.DefindID;
            data.IsEnabled = this.IsEnabled;
            data.WorkProcessType = this.WorkProcessType;
            data.IsWorkProcessNotice = this.IsWorkProcessNotice;
            data.HeaderQuarterNodes = this.ServingBu?.Select(buID => new Domain.Organization.HeaderQuarterNode()
            {
                NodeID = buID
            }).ToList() ?? new List<Domain.Organization.HeaderQuarterNode>();

            return data;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int? DefindID { get; set; }
        public string DefindName { get; set; }
        public string DefindKey { get; set; }
        public string CreateDateTime { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateDateTime { get; set; }
        public string UpdateUserName { get; set; }

        public int? CallCenterID { get; set; }
        public string CallCenterName { get; set; }

        public WorkProcessType WorkProcessType { get; set; }

        public bool IsWorkProcessNotice { get; set; }

        public List<int> ServingBu { get; set; }

        public List<NodeJobListViewModel> Jobs { get; set; }
    }
}
