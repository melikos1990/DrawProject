using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseFinishReason
{
    public class CaseFinishDataListViewModel
    {
        public CaseFinishDataListViewModel()
        {
        }

        public CaseFinishDataListViewModel(CaseFinishReasonData data)
        {
            this.ID = data.ID;
            this.ClassificationID = data.ClassificationID;
            this.Text = data.Text;
            this.IsEnabled = data.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
            this.IsMutiple = data.CaseFinishReasonClassification?.IsMultiple.DisplayBit();
            this.ClassificationName = data.CaseFinishReasonClassification?.Title;
            this.Order = data.Order;
            this.Default = data.Default;
            this.NodeName = data.NodeName;
            this.NodeID = data.NodeID;
            this.OrganizationType = data.OrganizationType;
        }

        public CaseFinishReasonData ToDomain()
        {
            var result = new CaseFinishReasonData();

            result.ID = this.ID;
            result.ClassificationID = this.ClassificationID;
            result.OrganizationType = this.OrganizationType;

            return result;
        }

        /// <summary>
        /// 分類代號
        /// </summary>
        public int ClassificationID { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string ClassificationName { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsEnabled { get; set; }

        /// <summary>
        /// 企業別代號
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
        /// 明細名稱
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否預選
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// 是否多選
        /// </summary>
        public string IsMutiple { get; set; }
    }
}
