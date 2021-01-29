using System;
using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Notification
{
    public class NotificationGroup : INullableItemRelationship, IOrganizationRelationship, INullableQuestionClassificationRelationship
    {
        public NotificationGroup()
        {
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 組織節點代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 是否達標
        /// </summary>
        public bool IsArrive { get; set; }

        /// <summary>
        /// 示警週期
        /// </summary>
        public int AlertCycleDay { get; set; }

        /// <summary>
        /// 計算模式 (0 : 商品 ; 1:問題分類 ; 2 : 兩者都)
        /// </summary>
        public NotificationCalcType CalcMode { get; set; }

        /// <summary>
        /// 該規則是否已通知過了
        /// </summary>
        public bool IsNotificated { get; set; }

        /// <summary>
        /// 示警次數
        /// </summary>
        public int AlertCount { get; set; }

        /// <summary>
        /// 實際達標數
        /// </summary>
        public int ActualCount { get; set; }

        /// <summary>
        /// 提醒群組人員
        /// </summary>
        public List<NotificationGroupUser> NotificationGroupUsers { get; set; }

        /// <summary>
        /// 提醒群組發送歷程
        /// </summary>
        public List<NotificationGroupResume> NotificationGroupResume { get; set; }

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
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

     

        #region impl

        /// <summary>
        /// 商品代號
        /// </summary>
        public int? ItemID { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織
        /// </summary>
        public IOrganizationNode Node { get; set; }

        /// <summary>
        /// 問題分類代號
        /// </summary>
        public int? QuestionClassificationID { get; set; }

        /// <summary>
        /// 問題分類名稱
        /// </summary>
        public string QuestionClassificationName { get; set; }

        /// <summary>
        /// 問題分類父名稱
        /// </summary>
        public string QuestionClassificationParentNames { get; set; }

        /// <summary>
        /// 問題分類父代號
        /// </summary>
        public string QuestionClassificationParentPath { get; set; }

        /// <summary>
        /// 問題分類父名稱陣列
        /// </summary>
        public string[] QuestionClassificationParentNamesByArray { get; set; }

        /// <summary>
        /// 問題分類父代號陣列
        /// </summary>
        public string[] QuestionClassificationParentPathByArray { get; set; }

        #endregion impl

        [JsonIgnore]
        [IgnoreMap]
        public string[] TargetNames
        {
            get
            {
                var targets = new List<string>();

                if (QuestionClassificationParentNamesByArray != null)
                {
                    targets.AddRange(QuestionClassificationParentNamesByArray);
                }
                if (string.IsNullOrEmpty(ItemName) == false)
                {
                    targets.Add(ItemName);
                }

                return targets?.ToArray();
            }
        }
    }
}
