using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NodeJobListViewModel
    {
        public NodeJobListViewModel()
        {
        }

        public NodeJobListViewModel(JobPosition data)
        {
            this.NodeID = data.NodeID;
            this.NodeJobID = data.ID;
            this.Name = data.Job.Name;
            this.ID = data.Job.ID;
            this.IsEnabled = data.Job.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
            this.Level = data.Job.Level.ToString();
            this.Users = data.Users?.Select(x => new NodeJobUserListViewModel(x)).ToList();
        }

        /// <summary>
        /// 組織節點代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 職稱代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 結點職稱代號
        /// </summary>
        public int NodeJobID { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用名稱
        /// </summary>
        public string IsEnabled { get; set; }

        /// <summary>
        /// 職稱階級
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 底下人員
        /// </summary>
        public List<NodeJobUserListViewModel> Users { get; set; } = new List<NodeJobUserListViewModel>();
    }
}
