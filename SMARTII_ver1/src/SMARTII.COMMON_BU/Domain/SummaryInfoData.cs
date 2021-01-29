using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.System;

namespace SMARTII.COMMON_BU
{
    public class SummaryInfoData
    {
        /// <summary>
        /// 所有問題分類項目(大分類)
        /// </summary>
        public List<QuestionClassification> QuestionClassifications = new List<QuestionClassification>();

        /// <summary>
        /// 每個大項目問題分類底下案件
        /// </summary>
        public List<(int ClassifictionID, List<Case> cases)> Cases = new List<(int ClassifictionID, List<Case> cases)>();

        /// <summary>
        /// 來源項目種類
        /// </summary>
        public List<SelectItem> SelectItems;      
    }
}
