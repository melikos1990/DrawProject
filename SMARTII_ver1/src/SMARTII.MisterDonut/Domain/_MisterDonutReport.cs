using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;

namespace SMARTII.MisterDonut.Domain
{
    public class _MisterDonutReport : BaseComplaintReport
    {

        public string Unit { get; set; }
        /// <summary>
        /// 區域名稱
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 門市名稱
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 區顧問
        /// </summary>
        public string Supervisor { get; set; }
        /// <summary>
        /// 是否為急件 否則為速件
        /// </summary>
        public Nullable<bool> IsDispatch { get; set; }
        //反應內容
        /// <summary>
        /// 服務態度
        /// </summary>
        public bool ServiceAttitude { get; set; }
        public List<string> ServiceAttitudes = new List<string> { "TH0129", "TH0130", "TH0131", "TH0132" };
        /// <summary>
        /// 服務瑕疵
        /// </summary>
        public bool ServiceDefect { get; set; }
        public List<string> ServiceDefects = new List<string> { "TH0133", "TH0134", "TH0135", "TH0136", "TH0137", "TH0138", "TH0139", "TH0140", "TH0160" };
        /// <summary>
        /// 商品異常
        /// </summary>
        public bool ProductAnomaly { get; set; }
        public List<string> ProductAnomalys = new List<string> { "TH0143", "TH0145" };
        /// <summary>
        /// 其他
        /// </summary>
        public bool Other { get; set; }      
        public List<string> Others = new List<string> { "TH0141", "TH0144", "TH0146", "TH0147", "TH0154" };

    }

    
}
