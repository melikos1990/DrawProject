using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;

namespace SMARTII.PPCLIFE.Domain
{
    public class _PPCLifeReport : BaseComplaintReport
    {
        /// <summary>
        /// 受理單位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 品牌名稱
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 反應內容清單
        /// </summary>
        public List<Tuple<string, bool>> ResponesList { get; set; }
    }
}
