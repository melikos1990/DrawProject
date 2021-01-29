using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NotificationSenderListViewModel
    {
        private PPCLifeEffectiveSummary x;

        public NotificationSenderListViewModel(PPCLifeEffectiveSummary x)
        {
            this.EffectiveID = x.ID;
            this.PPCLifeArriveType = x.PPCLifeArriveType.GetDescription();
            this.ItemID = x.ItemID;
            this.BatchNo = x.BatchNo;
            this.ArriveCount = x.CasePPCLifes.Count();
        }

        /// <summary>
        /// 流水編號      
        /// </summary>
        public int EffectiveID { get; set; }

        /// <summary>
        /// 達標規則
        /// </summary>
        public string PPCLifeArriveType { get; set; }

        /// <summary>
        /// 商品編號      
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 商品條碼
        /// </summary>
        public string InternationalBarcode { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string CommodityName { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 達標數
        /// </summary>
        public int ArriveCount { get; set; }
    }
}
