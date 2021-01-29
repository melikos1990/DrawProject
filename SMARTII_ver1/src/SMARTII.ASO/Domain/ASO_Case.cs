using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.ASO.Domain
{

    [JsonObject(MemberSerialization.OptIn)]
    public class ASO_Case
    {
        /// <summary>
        /// 商品
        /// </summary>
        [JsonProperty]
        public List<ASO_CaseItem> CaseItem { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ASO_CaseItem : CaseItem
    {
        /// <summary>
        /// 型號
        /// </summary>
        [JsonProperty]
        public string ProductModel { get; set; }

        /// <summary>
        /// 名稱  * ASO 沒用商品主檔 所以名稱列入動態欄位 *
        /// </summary>
        [JsonProperty]
        public string ProductName { get; set; }

        /// <summary>
        /// 購買日
        /// </summary>
        [JsonProperty]
        public DateTime? PurchaseDay { get; set; }

    }

}
