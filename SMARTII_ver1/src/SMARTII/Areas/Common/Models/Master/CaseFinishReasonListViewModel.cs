using System.Collections.Generic;
using System.Linq;
using SMARTII.Areas.Master.Models.CaseFinishReason;
using SMARTII.Domain.Case;

namespace SMARTII.Areas.Common.Models.Master
{
    public class CaseFinishReasonListViewModel
    {
        public CaseFinishReasonListViewModel()
        {
        }

        public CaseFinishReasonListViewModel(CaseFinishReasonClassification finishReasonClassification)
        {
            this.ClassificationName = finishReasonClassification.Title;
            this.ClassificationID = finishReasonClassification.ID;
            this.Order = finishReasonClassification.Order;

            this.FinishReasons = finishReasonClassification.CaseFinishReasonDatas.Select(x => new CaseFinishDataDetailViewModel
            {
                ClassificationID = x.ClassificationID,
                Text = x.Text,
                ID = x.ID,
                Default = x.Default,
                Order = x.Order
            }).OrderBy(x => x.Order).ToList();
        }

        /// <summary>
        /// 處理原因類型
        /// </summary>
        public string ClassificationName { get; set; }

        /// <summary>
        /// 處理原因ID
        /// </summary>
        public int ClassificationID { get; set; }

        /// <summary>
        /// 處理方式名稱
        /// </summary>
        public List<CaseFinishDataDetailViewModel> FinishReasons { get; set; }

        public int Order { get; set; }
    }
}
