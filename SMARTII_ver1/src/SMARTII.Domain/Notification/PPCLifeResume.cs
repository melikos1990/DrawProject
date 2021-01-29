using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Notification
{
    public class PPCLifeResume
    {
        /// <summary>
        /// 流水編號      
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 達標規則
        /// </summary>
        public PPCLifeArriveType PPCLifeArriveType { get; set; }

        /// <summary>
        /// 發送內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 商品 - 國際條碼
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 發送結果
        /// </summary>
        public NotificationGroupResultType NotificationGroupResultType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string EMLFilePath { get; set; }
    }
}
