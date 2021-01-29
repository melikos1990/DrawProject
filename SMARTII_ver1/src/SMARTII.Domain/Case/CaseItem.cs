using System.ComponentModel;
using AutoMapper;
using Newtonsoft.Json;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;

namespace SMARTII.Domain.Case
{

    public class CaseItem : DynamicSerializeObject
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        [JsonProperty]
        [Description("案件編號")]
        public string CaseID { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        [JsonProperty]
        [Description("商品編號")]
        public int ItemID { get; set; }

        /// <summary>
        /// 相關案件
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// 相關商品
        /// </summary>
        [Description("相關商品")]
        public Item Item { get; set; }

       
    }
}
