using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailAdminOrderViewModel
    {
        public OfficialEmailAdminOrderViewModel()
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
        /// 指派對象
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// GROUP代號
        /// </summary>
        [Required]
        public int GroupID { get; set; }
    }
}
