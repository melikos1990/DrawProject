using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMARTII.Areas.Master.Models.OfficialEmailGroup
{
    public class OfficialEmailGroupListViewModel
    {
        public OfficialEmailGroupListViewModel(Domain.Notification.OfficialEmailGroup data)
        {
            this.BuID = data.NodeID;
            this.BuName = data.NodeName;
            this.ID = data.ID;
            this.Account = data.Account;
            this.UserNames = data.User.Select(x => x.Name).ToArray();
            this.IsEnabled = data.IsEnabled ? "啟用" : "停用";
        }

        public int? BuID { get; set; }

        public string BuName { get; set; }
        
        public int ID { get; set; }

        public string Account { get; set; }

        public string[] UserNames { get; set; }

        public string IsEnabled { get; set; }

    }
}
