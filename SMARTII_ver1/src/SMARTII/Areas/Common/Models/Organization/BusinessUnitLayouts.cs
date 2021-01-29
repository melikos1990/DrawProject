using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Areas.Master.Models.CaseFinishReason;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.System;

namespace SMARTII.Areas.Common.Models.Organization
{
    public class BusinessUnitLayouts
    {

        public BusinessUnitLayouts() { }

        public BusinessUnitLayouts(List<SystemParameter> systemParameters, List<CaseFinishReasonClassification> classifications)
        {
            this.StoreDetailLayout = systemParameters.FirstOrDefault(x => x.ID == EssentialCache.LayoutValue.StoreDeatilTemplate)?.Value;
            this.StoreQueryLayout = systemParameters.FirstOrDefault(x => x.ID == EssentialCache.LayoutValue.StoreQueryTemplate)?.Value;
            this.ItemDetailLayout = systemParameters.FirstOrDefault(x => x.ID == EssentialCache.LayoutValue.ItemDeatilTemplate)?.Value;
            this.ItemQueryLayout = systemParameters.FirstOrDefault(x => x.ID == EssentialCache.LayoutValue.ItemQueryTemplate)?.Value;
            this.CaseOtherLayout = systemParameters.FirstOrDefault(x => x.ID == EssentialCache.LayoutValue.CaseOtherTemplate)?.Value;
            this.CaseFinishReasonClassifications = classifications?.Select(x=> new CaseFinishClassificationListViewModel(x))
                                                                   .ToList();
        }

        /// <summary>
        /// 門市進階查詢
        /// </summary>
        public string StoreQueryLayout { get; set; }

        /// <summary>
        /// 門市明細
        /// </summary>
        public string StoreDetailLayout { get; set; }

        /// <summary>
        /// 商品進階查詢
        /// </summary>
        public string ItemQueryLayout { get; set; }

        /// <summary>
        /// 商品明細
        /// </summary>
        public string ItemDetailLayout { get; set; }

        /// <summary>
        /// 案件明細
        /// </summary>
        public string CaseOtherLayout { get; set; }

        /// <summary>
        /// 處置原因分類
        /// ※ 須包含內容 (CaseFinishReasonData)
        /// </summary>
        public List<CaseFinishClassificationListViewModel> CaseFinishReasonClassifications { get; set; }
    }
}
