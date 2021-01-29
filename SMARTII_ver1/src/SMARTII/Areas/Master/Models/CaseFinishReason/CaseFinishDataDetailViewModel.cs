using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseFinishReason
{
    public class CaseFinishDataDetailViewModel
    {
        public CaseFinishDataDetailViewModel()
        {
        }

        public CaseFinishDataDetailViewModel(Domain.Case.CaseFinishReasonData data)
        {
            this.ClassificationID = data.ClassificationID;
            this.ID = data.ID;
            this.Text = data.Text;
            this.IsEnabled = data.IsEnabled;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.Order = data.Order;
            this.Default = data.Default;
            this.NodeID = data.NodeID;
            this.NodeName = data.NodeName;
        }

        public Domain.Case.CaseFinishReasonData ToDomain()
        {
            var result = new Domain.Case.CaseFinishReasonData();
            result.ID = this.ID;
            result.ClassificationID = this.ClassificationID;
            result.Text = this.Text;
            result.IsEnabled = this.IsEnabled;
            result.Order = this.Order;
            result.Default = this.Default;
            return result;
        }

        /// <summary>
        /// 分類代號
        /// </summary>
        public int ClassificationID { get; set; }

        /// <summary>
        /// 是否預選
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 組織型態定義
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 處理名稱
        /// </summary>
        public string Text { get; set; }

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
