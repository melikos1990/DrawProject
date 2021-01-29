using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;

namespace SMARTII.PPCLIFE.Models.NotificationSender
{
    public class NotificationSenderResumeListViewModel
    {
        private PPCLifeResume x;

        public NotificationSenderResumeListViewModel(PPCLifeResume x)
        {
            this.PPCLifeArriveType = x.PPCLifeArriveType.GetDescription();
            this.InternationalBarcode = x.ItemCode;
            this.ItemName = x.ItemName;
            this.BatchNo = x.BatchNo;
            this.Content = x.Content;
            this.NotificationGroupResultType = x.NotificationGroupResultType.GetDescription();
            this.CreateUserName = x.CreateUserName;
            this.CreateDateTime = x.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.EMLFilePath = x.EMLFilePath;
        }

        /// <summary>
        /// 達標規則
        /// </summary>
        public string  PPCLifeArriveType { get; set; }

        /// <summary>
        /// 商品條碼
        /// </summary>
        public string InternationalBarcode { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 通知內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 發送結果
        /// </summary>
        public string NotificationGroupResultType { get; set; }
        
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string  CreateDateTime { get; set; }


        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string EMLFilePath { get; set; }

    }
}
