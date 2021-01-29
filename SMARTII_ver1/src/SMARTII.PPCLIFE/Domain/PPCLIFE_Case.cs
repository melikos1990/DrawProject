using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SMARTII.Domain.Case;

namespace SMARTII.PPCLIFE.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PPCLIFE_Case 
    {
        /// <summary>
        /// 商品
        /// </summary>
        [JsonProperty]
        public List<PPCLIFE_CaseItem> CaseItem { get; set; }

    }
    [JsonObject(MemberSerialization.OptIn)]

    public class PPCLIFE_CaseItem : CaseItem
    {
        /// <summary>
        /// 批號
        /// </summary>
        [JsonProperty]
        public string BatchNo { get; set; }


    }
}
