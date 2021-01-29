using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailAutoOrderViewModel
    {
        public OfficialEmailAutoOrderViewModel()
        {
        }

        /// <summary>
        /// 指派案件數/人
        /// </summary>
        public int? EachPersonMail { get; set; }

        /// <summary>
        /// 指派對象
        /// </summary>
        public string[] UserIDs { get; set; }

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
