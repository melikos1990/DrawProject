using SMARTII.Domain.Data;

namespace SMARTII.Areas.Organization.Models.Enterprise
{
    public class EnterpriseListViewModel
    {
        public EnterpriseListViewModel()
        {
        }

        public EnterpriseListViewModel(Domain.Organization.Enterprise enterprise)
        {
            this.EnterpriseID = enterprise.ID;
            this.EnterpriseName = enterprise.Name;
            this.CreateUserName = enterprise.CreateUserName;
            this.CreateDateTime = enterprise.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.IsEnabled = enterprise.IsEnabled.DisplayBit(); ;
        }

        /// <summary>
        /// 企業代號
        /// </summary>
        public int EnterpriseID { get; set; }

        /// <summary>
        /// 企業名稱
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
        /// <summary>
        /// 啟用/停用
        /// </summary>
        public string IsEnabled { get; set; }

    }
}
