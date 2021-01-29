using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.ICC.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InComm_Case
    {
        /// <summary>
        /// 商品
        /// </summary>
        [JsonProperty]
        public List<InComm_CaseItem> CaseItem { get; set; }
    }

    public class InComm_CaseItem : CaseItem
    {
        /// <summary>
        /// 卡號
        /// </summary>
        [JsonProperty]
        public string CardNumber { get; set; }
    }

}
