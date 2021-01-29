using System;
using System.Collections.Generic;

namespace SMARTII.Domain.Organization
{
    public class CallCenterNode : OrganizationNodeBase, IExecutiveOrganizationNode
    {
  

        /// <summary>
        /// 組織深度(高=>低)
        /// </summary>
        public int Level { get; set; }

      
        /// <summary>
        /// 客服節點
        /// </summary>
        public int? CallCenterID { get; set; }

        /// <summary>
        /// 工作模式(單人/偕同)
        /// </summary>
        public WorkProcessType WorkProcessType { get; set; }

        /// <summary>
        /// 是否工作通知
        /// </summary>
        public bool IsWorkProcessNotice { get; set; }

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

        /// <summary>
        /// 組織型態定義 (HEADQUARTER : 0 ; CC : 1 ; VENDOR : 2)
        /// </summary>
        public OrganizationType OrganizationType => OrganizationType.CallCenter;

        #region refrence

        /// <summary>
        /// 組織階層定義檔
        /// </summary>
        public OrganizationNodeDefinition OrganizationNodeDefinitaion { get; set; }

        /// <summary>
        /// 以 客服節點來說 , 切分點是客服中心
        /// 這邊指的是所屬的 客服中心 結點
        /// </summary>
        public CallCenterNode CallCenterParent { get; set; }

        /// <summary>
        ///  以 客服節點來說 , 切分點是客服中心
        /// 這邊指的是客服中心 下的所有結點
        /// </summary>
        public List<CallCenterNode> CallCenterChildren { get; set; }

        /// <summary>
        /// 業務承接對象
        /// 這邊指的是 BU
        /// </summary>
        public List<HeaderQuarterNode> HeaderQuarterNodes { get; set; }

        /// <summary>
        /// Queue 清單
        /// </summary>

        public List<Domain.Master.Queue> Queues { get; set; }

        #endregion refrence
    }
}
