using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMARTII.Areas.Case.Models.OfficialEmail
{
    public class OfficialEmailReplyRengeViewModel
    {
        public OfficialEmailReplyRengeViewModel()
        {
        }

        /// <summary>
        /// 問題分類編號
        /// </summary>
        public int QuestionID { get; set; }

        /// <summary>
        /// 信件回覆內容
        /// </summary>
        public string EmailContent { get; set; }

        /// <summary>
        /// 結案內容
        /// </summary>
        public string FinishContent { get; set; }

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
