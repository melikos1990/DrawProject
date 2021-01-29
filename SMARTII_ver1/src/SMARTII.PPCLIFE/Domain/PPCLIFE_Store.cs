using System;
using Newtonsoft.Json;
using SMARTII.Domain.Organization;

namespace SMARTII.PPCLIFE.Domain
{
    [JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class PPCLIFE_Store : Store<PPCLIFE_Item>
    {
        
        [JsonProperty]
        public Boolean CreditCard { get; set; }

        [JsonProperty]
        public string MobilePay { get; set; }

        [JsonProperty]
        public Boolean Park { get; set; }

        [JsonProperty]
        public string OrderLimit { get; set; }
        [JsonProperty]
        public Boolean Toilet { get; set; }

        [JsonProperty]
        public string NumberTable { get; set; }
        [JsonProperty]
        public Boolean Delivery { get; set; }

        [JsonProperty]
        public string PurchaseDate { get; set; }          
    }
}
