using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseWarning
{
    public class CaseWarningListViewModel
    {
        public CaseWarningListViewModel()
        {
        }

        public CaseWarningListViewModel(Domain.Case.CaseWarning data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
            this.NodeID = data.NodeID;
            this.NodeName = data.NodeName;
            this.OrganizationType = data.OrganizationType;
            this.IsEnabled = data.IsEnabled.DisplayBit(@true:"啟用" , @false:"停用");
            this.WorkHour = data.WorkHour;
            this.Order = data.Order;
        }
        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 組織型態定義
        /// </summary>
        public OrganizationType OrganizationType { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsEnabled { get; set; }
        /// <summary>
        /// 等級時效(小時)
        /// </summary>
        public int WorkHour { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
