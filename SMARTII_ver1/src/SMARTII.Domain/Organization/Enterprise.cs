using System;

namespace SMARTII.Domain.Organization
{
    public class Enterprise
    {
        /// <summary>
        /// 集團別代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 集團別名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
