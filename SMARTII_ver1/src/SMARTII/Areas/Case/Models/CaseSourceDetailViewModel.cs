using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseSourceDetailViewModel
    {
        public CaseSourceDetailViewModel()
        {
        }

        public CaseSourceDetailViewModel(CaseSource caseSource)
        {
            this.GroupID = caseSource.GroupID;
            this.SourceID = caseSource.SourceID;
            this.IsPrevention = caseSource.IsPrevention;
            this.IsTwiceCall = caseSource.IsTwiceCall;
            this.NodeID = caseSource.NodeID;
            this.OrganizationType = caseSource.OrganizationType;
            this.RelationCaseIDs = caseSource.RelationCaseIDs;
            this.Remark = caseSource.Remark;
            this.CaseSourceType = caseSource.CaseSourceType;
            this.VoiceID = caseSource.VoiceID;
            this.VoiceLocator = caseSource.VoiceLocator;
            this.CreateDateTime = caseSource.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = caseSource.CreateUserName;
            this.UpdateDateTime = caseSource.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = caseSource.UpdateUserName;
            this.IncomingDateTime = caseSource.IncomingDateTime;

            this.User = new CaseSourceUserViewModel(caseSource?.CaseSourceUser);

            this.RelationCaseSourceID = caseSource.RelationCaseSourceID;
            this.RelationNodeID = (caseSource as ICaseSourceRelationship)?.CaseSource?.CaseSourceUser?.NodeID;
            this.RelationNodeName = (caseSource as ICaseSourceRelationship)?.CaseSource?.CaseSourceUser?.NodeName;

            this.Cases = caseSource?.Cases?
                                    .Select(x => new CaseDetailViewModel(x))
                                    .ToList();
        }

        public CaseSource ToDomain()
        {
            var result = new CaseSource();

            result.Cases = this.Cases?
                               .Select(x => x.ToDomain())
                               .ToList();

            result.GroupID = this.GroupID;
            result.CaseSourceType = this.CaseSourceType;
            result.CaseSourceUser = this.User.ToDomain();
            result.IncomingDateTime = this.IncomingDateTime;
            result.IsPrevention = this.IsPrevention;
            result.IsTwiceCall = this.IsTwiceCall;
            result.NodeID = this.NodeID;
            result.OrganizationType = this.OrganizationType;
            result.RelationCaseIDs = this.RelationCaseIDs;
            result.Remark = this.Remark;
            result.SourceID = this.SourceID;
            result.VoiceID = this.VoiceID;
            result.VoiceLocator = this.VoiceLocator;
            result.RelationCaseSourceID = this.RelationCaseSourceID;

            return result;
        }

        /// <summary>
        /// 案件來源編號
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// 是否二次來電
        /// </summary>
        public bool IsTwiceCall { get; set; }

        /// <summary>
        /// 是否為預立案件
        /// </summary>
        public bool IsPrevention { get; set; }

        /// <summary>
        /// 執行單位
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 勾稽案件編號
        /// </summary>
        public string[] RelationCaseIDs { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 案件來源
        /// </summary>
        public CaseSourceType CaseSourceType { get; set; }

        /// <summary>
        /// 錄音帶號
        /// </summary>
        public string VoiceID { get; set; }

        /// <summary>
        /// 音檔位址
        /// </summary>
        public string VoiceLocator { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 新增人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 預立案代號
        /// </summary>
        public string RelationCaseSourceID { get; set; }

        /// <summary>
        /// 預立案單位名稱
        /// </summary>
        public string RelationNodeName { get; set; }

        /// <summary>
        /// 預立案單位代號
        /// </summary>
        public int? RelationNodeID { get; set; }

        /// <summary>
        /// 來源時間
        /// </summary>
        public DateTime? IncomingDateTime { get; set; }

        /// <summary>
        /// 反應者資訊
        /// </summary>
        public CaseSourceUserViewModel User { get; set; }

        /// <summary>
        /// 底下的案件
        /// </summary>
        public List<CaseDetailViewModel> Cases { get; set; }
    }
}
