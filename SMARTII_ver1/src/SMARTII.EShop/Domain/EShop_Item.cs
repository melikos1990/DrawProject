using System;
using Newtonsoft.Json;
using SMARTII.Domain.Master;

namespace SMARTII.EShop.Domain
{
    [JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class EShop_Item : Item<EShop_Item>
    {
        [JsonProperty]
        public string Channel { get; set; }
        /// <summary>
        /// 國際條碼
        /// </summary>
        [JsonProperty]
        public string InternationalBarcode { get; set; }

        [JsonProperty]
        public Boolean CanReturn { get; set; }

        [JsonProperty]
        public DateTime? StopSalesDateTime { get; set; }
    }
}
