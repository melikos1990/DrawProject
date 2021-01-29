using System;
using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseFinishReasonClassification : IOrganizationRelationship
    {
        /// <summary>
        /// 分類代號
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

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
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否多選
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 結案原因 識別值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 所擁有的結案處置
        /// </summary>
        public virtual List<CaseFinishReasonData> CaseFinishReasonDatas { get; set; }

        #region impl
      
        public OrganizationType OrganizationType { get; set; }
        public int NodeID { get; set; }
        public string NodeName { get; set; }
        public IOrganizationNode Node { get; set; }
        #endregion


    }
}
