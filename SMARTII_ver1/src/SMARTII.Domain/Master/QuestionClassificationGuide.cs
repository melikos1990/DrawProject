using System;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class QuestionClassificationGuide : IOrganizationRelationship
    {
        public QuestionClassificationGuide()
        {
        }
        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 組織代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 提示內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public Boolean IsEnable { get; set; }

        /// <summary>
        /// 組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 分類代號
        /// </summary>
        public int ClassificationID { get; set; }
        /// <summary>
        /// 分類名稱
        /// </summary>
        public string ClassificationName { get; set; }
        
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

        public QuestionClassification QuestionClassification { get; set; }
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
