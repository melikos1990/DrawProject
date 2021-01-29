using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseTag
{
    public class CaseTagDetailViewModel
    {
        public CaseTagDetailViewModel()
        { }

        public CaseTagDetailViewModel(Domain.Case.CaseTag data)
        {
            this.BuID = data.NodeID;
            this.ID = data.ID;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled;
            this.OrganizationType = data.OrganizationType;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
        }

        /// <summary>
        /// 範例企業別
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 啟用/停用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// TAG名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 組織型態定義
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }
    }
}