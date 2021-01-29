using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NotificationSenderCaseListViewModel
    {
        private CasePPCLife x;
        public NotificationSenderCaseListViewModel()
        {

        }

        public NotificationSenderCaseListViewModel(CasePPCLife x,int EffectiveID)
        {
            this.EffectiveID = EffectiveID;
            this.CaseID = x.CaseID;
            this.ItemID = x.ItemID;
            this.CaseContent = x.CaseContent;
        }

        public CasePPCLife ToDomain()
        {
            var result = new CasePPCLife();
            result.EffectiveID = this.EffectiveID;
            result.ItemID = this.ItemID;
            result.CaseID = this.CaseID;

            return result;
        }

        /// <summary>
        /// 流水編號      
        /// </summary>
        public int EffectiveID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
    }
}
