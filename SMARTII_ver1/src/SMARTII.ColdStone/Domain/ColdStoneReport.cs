using System.Collections.Generic;
using SMARTII.Domain.Case;

namespace SMARTII.ColdStone.Domain
{
    public class ColdStoneReport : BaseComplaintReport
    {
        /// <summary>
        /// 區組
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 門市名稱
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 區經理
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 服務相關
        /// </summary>
        public bool AboutService { get; set; }
        public List<string> AboutServices { get; set; } = new List<string>() { "SE0052","SE0053","SE0054"};
        /// <summary>
        /// 商品面
        /// </summary>
        public bool AboutBrand { get; set; }
        public List<string> AboutBrands { get; set; } = new List<string>() { "SE0055"};
        /// <summary>
        /// 形象面
        /// </summary>
        public bool AboutImage { get; set; }
        public List<string> AboutImages { get; set; } = new List<string>() { "SE0056"};
        /// <summary>
        /// 其他
        /// </summary>
        public bool Other { get; set; }
    }
}
