using System;
using SMARTII.Domain.Authentication.Object;

namespace SMARTII.Domain.System
{
    public class SystemLog
    {
        /// <summary>
        /// 代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 功能註記
        /// </summary>
        public string FeatureTag { get; set; }

        /// <summary>
        /// 功能名稱
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// 使用內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserAccount { get; set; }

        /// <summary>
        /// 操作權限
        /// </summary>
        public AuthenticationType Operator { get; set; }
    }
}