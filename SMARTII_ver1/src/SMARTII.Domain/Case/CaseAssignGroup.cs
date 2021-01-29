using System;
using System.Collections.Generic;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignGroup : IOrganizationRelationship
    {
        public CaseAssignGroup()
        {
        }

        #region impl Node

        public OrganizationType OrganizationType { get; set; } = OrganizationType.HeaderQuarter;
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public IOrganizationNode Node { get; set; }

        #endregion impl Node

        /// <summary>
        /// 群組代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 類別(ex:派工群組/大量叫修)
        /// </summary>
        public CaseAssignGroupType CaseAssignGroupType { get; set; }

        public HeaderQuarterNode HeaderQuarterNode { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 派工群組人員
        /// </summary>
        public List<CaseAssignGroupUser> CaseAssignGroupUsers { get; set; }
    }
}
