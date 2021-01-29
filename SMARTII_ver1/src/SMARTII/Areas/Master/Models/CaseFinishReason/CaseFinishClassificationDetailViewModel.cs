using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseFinishReason
{
    public class CaseFinishClassificationDetailViewModel
    {
        public CaseFinishClassificationDetailViewModel()
        {
        }

        public CaseFinishClassificationDetailViewModel(Domain.Case.CaseFinishReasonClassification data)
        {
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.ID = data.ID;
            this.IsEnabled = data.IsEnabled;
            this.IsMultiple = data.IsMultiple;
            this.NodeID = data.NodeID;
            this.Order = data.Order;
            this.OrganizationType = data.OrganizationType;
            this.Title = data.Title;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
            this.IsRequired = data.IsRequired;
        }

        public Domain.Case.CaseFinishReasonClassification ToDomain()
        {
            var result = new Domain.Case.CaseFinishReasonClassification();
            result.ID = this.ID;
            result.NodeID = this.NodeID;
            result.OrganizationType = this.OrganizationType;
            result.Order = this.Order;
            result.Title = this.Title;
            result.IsEnabled = this.IsEnabled;
            result.IsMultiple = this.IsMultiple;
            result.IsRequired = this.IsRequired;
            return result;
        }

        /// <summary>
        /// 識別值
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態定義
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 標題名稱
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否多選
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

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

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }
    }
}
