using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Case;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseFinishReason
{
    public class CaseFinishClassificationListViewModel
    {
        public CaseFinishClassificationListViewModel()
        {
        }

        public CaseFinishClassificationListViewModel(CaseFinishReasonClassification data)
        {
            this.ID = data.ID;
            this.NodeID = data.NodeID;
            this.Order = data.Order;
            this.OrganizationType = data.OrganizationType;
            this.Title = data.Title;
            this.IsEnabled = data.IsEnabled;
            this.IsMultiple = data.IsMultiple;
            this.CaseFinishDatas = data.CaseFinishReasonDatas?
                                      .Select(x => new CaseFinishDataListViewModel(x))
                                      .ToList();
        }

        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

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
        /// 案件結案資料
        /// </summary>
        public List<CaseFinishDataListViewModel> CaseFinishDatas { get; set; }
    }
}
