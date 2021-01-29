using System;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;

namespace SMARTII.Domain.Case
{
    public class CaseTemplate : IOrganizationRelationship, ISystemParameterRelationship
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 時機分類
        /// </summary>
        public string ClassificKey { get; set; }

        /// <summary>
        /// 時機分類值
        /// </summary>
        public string ClassificValue { get; set; }
        /// <summary>
        /// 時機分類名稱
        /// </summary>
        public string ClassificName { get; set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 新增人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 信件主旨
        /// </summary>
        public string EmailTitle { get; set; }

        /// <summary>
        /// 是否為預設
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 快速結案
        /// </summary>
        public bool IsFastFinished { get; set; }

        #region impl Node

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; } = OrganizationType.HeaderQuarter;

        /// <summary>
        /// 組織節點代號(企業別)
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織
        /// </summary>
        public IOrganizationNode Node { get; set; }

        #endregion impl Node

        #region impl system

        string ISystemParameterRelationship.ID { get; set; } = "CASE_TEMPLATE";

        string ISystemParameterRelationship.Key
        {
            get
            {
                return ClassificKey;
            }
            set
            {
                ClassificKey = value;
            }
        }

        string ISystemParameterRelationship.Value
        {
            get
            {
                return ClassificValue;
            }
            set
            {
                ClassificValue = value;
            }
        }

        string ISystemParameterRelationship.Text
        {
            get
            {
                return ClassificName;
            }
            set
            {
                ClassificName = value;
            }
        }

        #endregion impl system
    }
}
