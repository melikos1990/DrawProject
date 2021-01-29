using System;
using System.Collections.Generic;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class QuestionClassification: IOrganizationRelationship
    {
        public QuestionClassification()
        {
        }

        /// <summary>
        /// 組織節點
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 單位名稱
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 代號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 父階層代號
        /// </summary>
        public int? ParentID { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 父階層代號
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 擺放順序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 識別碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 更新人員姓名
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 第一層問題分類Code(來源: VW)
        /// </summary>
        public string FirstCode { get; set; }

        /// <summary>
        /// 範本清單
        /// </summary>
        public virtual List<QuestionClassificationAnswer> QuesionClassificationAnswer { get; set; }

        /// <summary>
        /// 流程導引
        /// </summary>
        public virtual QuestionClassificationGuide QuesionClassificationGuide { get; set; }

        /// <summary>
        /// 子分類
        /// </summary>
        public virtual List<QuestionClassification> Children { get; set; }

        /// <summary>
        /// 父階層分類
        /// </summary>
        public virtual QuestionClassification Parent { get; set; }

        /// <summary>
        /// 攤平底下的子類別(序列包含自己) enabled 有值時 如不符合就會跳過該節點底下的子節點
        /// </summary>
        /// <returns></returns>
        public List<QuestionClassification> Flatten(bool? enabled = null)
        {
            var result = new List<QuestionClassification>();

            if (enabled != null && this.IsEnabled != enabled)
                return result;

            if (this.Children != null && this.Children.Count > 0)
            {
                this.Children.ForEach(x =>
                {
                    result.AddRange(x.Flatten(enabled));
                });
            }

            result.Insert(0, this);

            return result;
        }

        /// <summary>
        /// 攤平父類別(序列包含自己)
        /// </summary>
        /// <returns></returns>
        public List<QuestionClassification> FlattenToUp()
        {
            var result = new List<QuestionClassification>();

            if (this.Parent != null)
            {
                var parent = this.Parent.FlattenToUp();
                result.AddRange(parent);
            }

            result.Insert(0, this);

            return result;
        }

        #region View Table

        /// <summary>
        /// 父節點的 名稱 路徑
        /// </summary>
        public string ParentNamePath { get; set; }

        /// <summary>
        /// 父節點的 ID 路徑
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// 父節點 的 名稱路徑清單
        /// </summary>
        public string[] ParentNamePathByArray { get { return this.ParentNamePath?.Split('@') ?? new string[] { }; } }

        /// <summary>
        /// 父節點 的 代號路徑清單
        /// </summary>
        public string[] ParentPathByArray { get { return this.ParentPath?.Split('@') ?? new string[] { }; } }


        #endregion View Table


        #region impl IOrganizationRelationship

        /// <summary>
        /// Bu名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織節點
        /// </summary>
        public IOrganizationNode Node { get; set; }

        #endregion
    }
}
