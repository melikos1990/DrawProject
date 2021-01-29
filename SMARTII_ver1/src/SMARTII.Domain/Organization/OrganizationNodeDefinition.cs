using System;
using System.Collections.Generic;

namespace SMARTII.Domain.Organization
{
    public class OrganizationNodeDefinition
    {
        /// <summary>
        /// 節點定義代碼
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 組織型態定義 (CC / HEADQUARTER / VENDOR)
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 定義名稱(EX : 區/課/門市)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 識別值
        /// </summary>
        public int? Identification { get; set; }

        /// <summary>
        /// 識別名稱
        /// </summary>
        public string IdentificationName { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnabled { get; set; }

        /// <summary>
        /// 組織節點定義職稱
        /// </summary>
        public List<Job> Jobs { get; set; }

        /// <summary>
        /// 階層
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 組織定義KEY (EX:BU/STORE)
        /// </summary>
        public string Key { get; set; }
    }
}
