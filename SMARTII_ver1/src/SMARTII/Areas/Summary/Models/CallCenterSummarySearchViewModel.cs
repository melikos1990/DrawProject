using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMARTII.Areas.Summary.Models
{
    public class CallCenterSummarySearchViewModel
    {

        /// <summary>
        /// 企業別 ID
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 是否為個人
        /// </summary>
        public bool IsSelf { get; set; }


        /// <summary>
        /// 案件等級
        /// </summary>
        public int? WarningID { get; set; }

    }
}
