using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.CaseTag
{
    public class CaseTagListViewModel
    {
        public CaseTagListViewModel()
        {
        }

        public CaseTagListViewModel(Domain.Case.CaseTag data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
            this.BuID = data.NodeID;
            this.BuName = data.NodeName;
            this.IsEnabled = data.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
        }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 啟用/停用
        /// </summary>
        public string IsEnabled { get; set; }

        /// <summary>
        /// TAG名稱
        /// </summary>
        public string Name { get; set; }
    }
}
