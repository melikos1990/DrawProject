using System;
using System.Collections.Generic;
using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseSource : IOrganizationRelationship, ICaseSourceRelationship, IFlowable
    {
        public CaseSource()
        {
        }

        /// <summary>
        /// 來源代號
        /// </summary>
        [Description("來源代號")]
        public string SourceID { get; set; }

        /// <summary>
        /// 是否為二次來電
        /// </summary>
        [Description("是否為二次來電")]
        public bool IsTwiceCall { get; set; }

        /// <summary>
        /// 是否為預立案件
        /// </summary>
        [Description("是否為預立案件")]
        public bool IsPrevention { get; set; }

        /// <summary>
        /// 來源時間
        /// </summary>
        [Description("來源時間")]
        public DateTime? IncomingDateTime { get; set; }

        /// <summary>
        /// 反應內容
        /// </summary>
        [Description("反應內容")]
        public string Remark { get; set; }

        /// <summary>
        /// 關聯的案件代號
        /// </summary>
        [Description("關聯的案件代號")]
        public string[] RelationCaseIDs { get; set; }

        /// <summary>
        /// 勾稽的案件來源編號 (預立案)
        /// </summary>
        [Description("勾稽的案件來源編號")]
        public string RelationCaseSourceID { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Description("建立時間")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        [Description("建立人員")]
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
        /// 音檔代號
        /// </summary>
        [Description("音檔代號")]
        public string VoiceID { get; set; }

        /// <summary>
        /// 音檔路徑
        /// </summary>
        [Description("音檔路徑")]
        public string VoiceLocator { get; set; }

        /// <summary>
        /// 案件來源型態
        /// 來信/來電
        /// </summary>
        [Description("案件來源型態")]
        public CaseSourceType CaseSourceType { get; set; }

        /// <summary>
        /// 反應者
        /// </summary>
        [Description("反應者")]
        public CaseSourceUser CaseSourceUser { get; set; }

        /// <summary>
        /// 底下的案件
        /// </summary>
        [Description("底下的案件")]
        public List<Case> Cases { get; set; }

        /// <summary>
        /// 執行單位(CC)
        /// </summary>
        [Description("執行單位(CC)")]
        public int? GroupID { get; set; }

        #region impl

        /// <summary>
        /// 組織節點代號(企業別)
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 組織
        /// </summary>
        public IOrganizationNode Node { get; set; }

        string ICaseSourceRelationship.SourceID
        {
            get
            {
                return this.RelationCaseSourceID;
            }
            set
            {
                this.RelationCaseSourceID = value;
            }
        }

        CaseSource ICaseSourceRelationship.CaseSource { get; set; }

        #endregion impl
    }
}
