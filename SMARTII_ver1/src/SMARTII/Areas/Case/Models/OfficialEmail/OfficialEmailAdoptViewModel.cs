using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailAdoptViewModel
    {
        public OfficialEmailAdoptViewModel()
        {
        }

        /// <summary>
        /// 信件編號
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 服務群組
        /// </summary>
        public int GroupID { get; set; }
    }
}
