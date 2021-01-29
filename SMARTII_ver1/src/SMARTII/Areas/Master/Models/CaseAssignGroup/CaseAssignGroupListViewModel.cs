using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.CaseAssignGroup
{
    public class CaseAssignGroupListViewModel
    {
        public CaseAssignGroupListViewModel()
        {
        }

        public CaseAssignGroupListViewModel(Domain.Case.CaseAssignGroup caseAssignGroup)
        {
            this.ID = caseAssignGroup.ID;
            this.BuID = caseAssignGroup.NodeID;
            this.BuName = caseAssignGroup.NodeName;
            this.Name = caseAssignGroup.Name;
            this.CaseAssignGroupTypeName = caseAssignGroup.CaseAssignGroupType.GetDescription();
        }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 群組類型名稱
        /// </summary>
        public string CaseAssignGroupTypeName { get; set; }
    }
}