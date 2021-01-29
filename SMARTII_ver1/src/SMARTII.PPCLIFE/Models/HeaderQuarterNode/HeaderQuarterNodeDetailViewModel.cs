using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class HeaderQuarterNodeDetailViewModel
    {
        public HeaderQuarterNodeDetailViewModel()
        {
        }

        public HeaderQuarterNodeDetailViewModel(HeaderQuarterNode data)
        {
            this.ID = data.NodeID;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled;
            this.EnterpriseID = data.EnterpriseID;
            this.DefindID = data.NodeType;
            this.DefindName = data.OrganizationNodeDefinitaion?.Name;
            this.DefindKey = data.OrganizationNodeDefinitaion?.Key;
            this.CreateUserName = data.CreateUserName;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = data.UpdateUserName;
            this.BUID = data.BusinessParent?.NodeID;
            this.BUName = data.BusinessParent?.Name;
            this.BUkey = data.NodeKey;
            this.StoreName = data.Store?.Name;
            this.StoreCode = data.Store?.Code;
            this.CanModifyBuCode = string.IsNullOrEmpty(data.NodeKey);
            this.Jobs = data.JobPosition?
                            .Select(x => new NodeJobListViewModel(x))
                            .ToList();
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
        public int? BUID { get; set; }
        public string BUName { get; set; }
        public string BUkey { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public bool CanModifyBuCode { get; set; }

        public List<NodeJobListViewModel> Jobs { get; set; }
    }
}
