using System.Collections.Generic;
using SMARTII.Domain.Data;

namespace SMARTII.Domain.Organization
{
    public interface IExecutiveOrganizationNode : IOrganizationNode
    {
        List<HeaderQuarterNode> HeaderQuarterNodes { get; set; }
    }

    public interface IReceiveOrganizationNode : IOrganizationNode
    {
    }

    public interface IOrganizationNode : INSMNestedModel
    {
        /// <summary>
        /// 節點代號
        /// </summary>
        int NodeID { get; set; }

        /// <summary>
        /// 節點KEY
        /// 針對BU 設定
        /// </summary>
        string NodeKey { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 左節點
        /// </summary>
        new int LeftBoundary { get; set; }

        /// <summary>
        /// 右邊節點
        /// </summary>
        new int RightBoundary { get; set; }

        /// <summary>
        /// 層級
        /// </summary>
        new int Level { get; set; }

        /// <summary>
        /// 父層級節點
        /// </summary>
        new int? ParentLocator { get; set; }

        /// <summary>
        /// 父層級路徑
        /// </summary>
        new string ParentPath { get; set; }

        /// <summary>
        /// 節點定義
        /// </summary>
        int? NodeType { get; set; }

        /// <summary>
        /// 節點定義KEY
        /// 如 STORE、GROUP...
        /// </summary>
        string NodeTypeKey { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        OrganizationType OrganizationType { get; }

        /// <summary>
        /// 職稱清單
        /// </summary>
        List<Job> Jobs { get; }

        /// <summary>
        /// 使用者清單
        /// </summary>
        List<User> Users { get; }

        /// <summary>
        /// 節點定義
        /// </summary>
        OrganizationNodeDefinition OrganizationNodeDefinitaion { get; set; }
    }
}