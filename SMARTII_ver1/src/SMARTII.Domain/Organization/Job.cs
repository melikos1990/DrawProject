using System;

namespace SMARTII.Domain.Organization
{
    public class Job
    {
        /// <summary>
        /// 職稱代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 節點定義代號
        /// </summary>
        public int DefinitionID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnabled { get; set; }

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
        /// 職稱階級
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 職稱識別值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 所屬的組織定義
        /// </summary>
        public OrganizationNodeDefinition OrganizationNodeDefinitaion { get; set; }
    }
}
