using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SMARTII.Domain.Case;

namespace SMARTII.EShop.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EShop_Case
    {
        /// <summary>
        /// 商品
        /// </summary>
        [JsonProperty]
        public List<EShop_CaseItem> CaseItem { get; set; }

    }
    [JsonObject(MemberSerialization.OptIn)]

    public class EShop_CaseItem : CaseItem
    {
        /// <summary>
        /// 批號
        /// </summary>
        [JsonProperty]
        public string BatchNo { get; set; }


    }
}
