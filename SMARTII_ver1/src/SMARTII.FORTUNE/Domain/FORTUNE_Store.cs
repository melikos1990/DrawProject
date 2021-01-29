using System;
using Newtonsoft.Json;
using SMARTII.Domain.Organization;

namespace SMARTII.FORTUNE.Domain
{
    [JsonObject(Newtonsoft.Json.MemberSerialization.OptIn)]
    public class FORTUNE_Store : Store<FORTUNE_Store>
    {
        [JsonProperty]
        public string ChargegunType { get; set; }
        [JsonProperty]
        public string ChargeType { get; set; }
        [JsonProperty]
        public string MaxPower { get; set; }
        [JsonProperty]
        public string IsParkingFee { get; set; }
        [JsonProperty]
        public string ParkingFee { get; set; }
        [JsonProperty]
        public string CharingFee { get; set; }
    }
}
