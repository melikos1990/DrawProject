using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class QuestionClassificationAnswer : IOrganizationRelationship
    {
        public QuestionClassificationAnswer()
        {
        }

        /// <summary>
        /// 代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 所屬問題代號
        /// </summary>
        public int ClassificationID { get; set; }


        /// <summary>
        /// 範本主旨
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 範本內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 節點代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 新增人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 所屬問題分類
        /// </summary>
        public virtual QuestionClassification QuestionClassification { get; set; }

        public string NodeName { get; set; }
        public IOrganizationNode Node { get; set; }

        #region View Table

        /// <summary>
        /// 父節點的 問題名稱
        /// </summary>
        public string ParentPathName { get; set; }

        /// <summary>
        /// 父節點的 問題ID
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// 父節點 的 問題名稱清單
        /// </summary>
        public string[] ParentPathNameByArray { get { return this.ParentPathName?.Split('@') ?? new string[] { }; } }

        /// <summary>
        /// 父節點 的 問題ID清單
        /// </summary>
        public string[] ParentPathByArray { get { return this.ParentPath?.Split('@') ?? new string[] { }; } }
        
        #endregion
    }
}
