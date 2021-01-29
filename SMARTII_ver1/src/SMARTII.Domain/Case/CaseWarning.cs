using System;
using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseWarning : IOrganizationRelationship
    {
        /// <summary>
        /// 流水號
        /// </summary>
        [Description("流水號")]
        public int ID { get; set; }

        /// <summary>
        /// 組織代號
        /// </summary>
        [Description("組織代號")]
        public int NodeID { get; set; }

        /// <summary>
        /// 定義名稱
        /// </summary>
        [Description("定義名稱")]
        public string Name { get; set; }

        /// <summary>
        /// 工作時數
        /// </summary>
        [Description("工作時數")]
        public int WorkHour { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Description("是否啟用")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 案件時效排序
        /// </summary>
        [Description("案件時效排序")]
        public int Order { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        [Description("新增時間")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 新增人員
        /// </summary>
        [Description("新增人員")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Description("更新時間")]
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        [Description("更新人員")]
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)
        /// </summary>
        public OrganizationType OrganizationType { get; set; }
        public string NodeName { get ; set; }
        public IOrganizationNode Node { get; set ; }
    }
}
