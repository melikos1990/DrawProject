using System;
using System.Collections.Generic;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
namespace SMARTII.Domain.Case
{
    public class CaseNotice : ICaseRelationship,IOrganizationRelationship
    {
        public CaseNotice()
        {
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 案件負責人代號
        /// </summary>
        public string ApplyUserID { get; set; }

        /// <summary>
        /// 通知型態
        /// </summary>
        public CaseNoticeType CaseNoticeType { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        public Case @case { get; set; }

        #region impl Node

        public OrganizationType OrganizationType { get; set; } = OrganizationType.HeaderQuarter;
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public IOrganizationNode Node { get; set; }

        #endregion impl Node
    }
}
