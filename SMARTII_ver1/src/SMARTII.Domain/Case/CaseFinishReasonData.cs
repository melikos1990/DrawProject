using System;
using System.ComponentModel;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseFinishReasonData : IOrganizationRelationship
    {
        /// <summary>
        /// 處置代號
        /// </summary>
        [Description("處置代號")]
        public int ID { get; set; }

        /// <summary>
        /// 顯示文字
        /// </summary>
        [Description("顯示文字")]
        public string Text { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Description("是否啟用")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        [Description("更新人員")]
        public string UpdateUserName { get; set; }

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
        /// 分類代號
        /// </summary>
        [Description("分類代號")]
        public int ClassificationID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int Order { get; set; }

        /// <summary>
        /// 是否預設
        /// </summary>
        [Description("是否預設")]
        public bool Default { get; set; }

        /// <summary>
        /// 所屬的處置分類
        /// </summary>
        [Description("所屬的處置分類")]
        public virtual CaseFinishReasonClassification CaseFinishReasonClassification { get; set; }


        #region 

        public int NodeID
        {
            get
            {
                if (this.CaseFinishReasonClassification != null)
                {
                    return this.CaseFinishReasonClassification.NodeID;
                }
                return default(int);
            }
            set
            {
                if (this.CaseFinishReasonClassification != null)
                {
                    this.CaseFinishReasonClassification.NodeID = value;
                } 
            }
        }
        public OrganizationType OrganizationType
        {
            get
            {
                if (this.CaseFinishReasonClassification != null)
                {
                    return this.CaseFinishReasonClassification.OrganizationType;
                }
                return default(OrganizationType);
            }
            set
            {
                if (this.CaseFinishReasonClassification != null)
                {
                    this.CaseFinishReasonClassification.OrganizationType = value;
                }
            }
        }
        public string NodeName { get; set; }
     
        public IOrganizationNode Node { get; set; }

        #endregion


    }
}
