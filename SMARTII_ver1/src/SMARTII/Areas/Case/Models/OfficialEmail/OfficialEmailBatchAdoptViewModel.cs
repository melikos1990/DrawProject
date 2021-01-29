using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailBatchAdoptViewModel
    {
        public OfficialEmailBatchAdoptViewModel()
        {
        }

        /// <summary>
        /// 信件編號
        /// </summary>
        public string[] MessageIDs { get; set; }

        /// <summary>
        /// 企業別
        /// </summary>
        [Required]
        public int BuID { get; set; }

        /// <summary>
        /// GROUP代號
        /// </summary>
        [Required]
        public int GroupID { get; set; }
    }
}
