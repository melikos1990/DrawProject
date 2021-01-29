using System;
using System.Collections.Generic;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class Billboard : IUserImageRelationship
    {
        /// <summary>
        /// 代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 緊急程度
        /// </summary>
        public BillboardWarningType BillboardWarningType { get; set; }

        /// <summary>
        /// 建立人員代號
        /// </summary>
        public string CreateUserID { get; set; }

        /// <summary>
        /// 建立人員姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新人員代號
        /// </summary>
        public string UpdateUserID { get; set; }

        /// <summary>
        /// 更新人員姓名
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 生效日開始
        /// </summary>
        public DateTime ActiveStartDateTime { get; set; }

        /// <summary>
        /// 生效日結束
        /// </summary>
        public DateTime ActiveEndDateTime { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string[] FilePaths { get; set; }

        /// <summary>
        /// 實體檔案
        /// </summary>
        public List<HttpFile> Files { get; set; }

        /// <summary>
        /// 通知人員代號清單
        /// </summary>
        public List<string> UserIDs { get; set; }

        /// <summary>
        /// 通知人員清單
        /// ※ 該欄位透過二次查詢求得
        /// </summary>
        public List<User> Users { get; set; }

        public bool IsNotificaed { get; set; }
        /// <summary>
        /// 建立人員圖片路徑
        /// </summary>
        public string ImagePath { get; set; }
    }
}
