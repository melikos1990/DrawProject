using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseWarning
{
    public class CaseWarningDetailViewModel
    {
        public CaseWarningDetailViewModel()
        {
        }
        public Domain.Case.CaseWarning ToDomain()
        {
            var result = new Domain.Case.CaseWarning();
            result.ID = this.ID;
            result.Name = this.Name;
            result.NodeID = this.NodeID;
            result.IsEnabled = this.IsEnabled;
            result.OrganizationType = (byte)OrganizationType.HeaderQuarter;
            result.WorkHour = this.WorkHour;
            return result;
        }
        public CaseWarningDetailViewModel(Domain.Case.CaseWarning data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
            this.NodeID = data.NodeID;
            this.OrganizationType = data.OrganizationType;
            this.IsEnabled = data.IsEnabled;
            this.WorkHour = data.WorkHour;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
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
        /// 組織型態定義
        /// </summary>
        public OrganizationType OrganizationType { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 等級時效(小時)
        /// </summary>
        public int WorkHour { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }
        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }
    }
}
