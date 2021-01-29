using System;
using System.Collections.Generic;
using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseTag : IOrganizationRelationship
    {
        public CaseTag()
        {
        }

        /// <summary>
        /// key (流水號)
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Description("是否啟用")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Description("名稱")]
        public string Name { get; set; }



        /// <summary>
        /// 底下的案件
        /// </summary>
        public List<Case> Cases { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        [Description("建立人員")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Description("建立時間")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        [Description("更新人員")]
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Description("更新時間")]
        public DateTime? UpdateDateTime { get; set; }

        #region impl

        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public OrganizationType OrganizationType { get; set; }
        public IOrganizationNode Node { get; set; }

        #endregion

        #region dirty field

        public bool Target { get; set; }

        #endregion


    }
}
