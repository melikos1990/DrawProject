using System;
using System.Collections.Generic;

namespace SMARTII.Domain.Organization
{
    public class HeaderQuarterNode : OrganizationNodeBase, IReceiveOrganizationNode
    {



        /// <summary>
        /// 組織深度(高=>低)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 集團別代號
        /// </summary>
        public int? EnterpriseID { get; set; }

     
        /// <summary>
        /// 企業代號
        /// </summary>
        public int? BUID { get; set; }

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
        public OrganizationType OrganizationType => OrganizationType.HeaderQuarter;

        #region reference

        /// <summary>
        /// 組織階層定義
        /// </summary>
        public OrganizationNodeDefinition OrganizationNodeDefinitaion { get; set; }

        /// <summary>
        /// 以 總部來說 , 切分點是BU
        /// 這邊指的是所屬的 BU 結點
        /// </summary>
        public HeaderQuarterNode BusinessParent { get; set; }

        /// <summary>
        /// 以 總部來說 , 切分點是BU
        /// 這邊指的是 BU 下的所有結點
        /// </summary>
        public List<HeaderQuarterNode> BusinessChildren { get; set; }

        /// <summary>
        /// 合約對象
        /// 這邊指的是廠商
        /// </summary>
        public List<VendorNode> VendorNodes { get; set; }

        /// <summary>
        /// 業務承接對象
        /// 這邊指的是客服中心
        /// </summary>
        public List<CallCenterNode> CallCenterNodes { get; set; }

        /// <summary>
        /// 門市資訊
        /// </summary>
        public Store Store { get; set; }

        #endregion reference
    }
}
