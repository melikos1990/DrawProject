using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.OrganizarionNodeDefinition
{
    public class JobListViewModel
    {
        public JobListViewModel()
        {
        }

        public JobListViewModel(Job job)
        {
            this.ID = job.ID;
            this.Name = job.Name;
            this.DefinitionID = job.DefinitionID;
            this.DefinitionName = job.OrganizationNodeDefinitaion?.Name;
            this.IsEnabled = job.IsEnabled;
            this.IsEnabledName = job.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
            this.Level = job.Level;
            this.OrganizationType = job.OrganizationType;
            this.OrganizationTypeName = OrganizationType.GetDescription();
            this.CreateDateTime = job.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = job.CreateUserName;
            this.UpdateDateTime = job.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = job.UpdateUserName;
            this.Key = job.Key;
        }

        /// <summary>
        /// 職稱代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 節點定義代號
        /// </summary>
        public int DefinitionID { get; set; }

        /// <summary>
        /// 節點定義名稱
        /// </summary>
        public string DefinitionName { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否啟用名稱
        /// </summary>
        public string IsEnabledName { get; set; }

        /// <summary>
        /// 職稱階級
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 組織型態名稱
        /// </summary>
        public string OrganizationTypeName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 職稱識別值
        /// </summary>
        public string Key { get; set; }
    }
}
