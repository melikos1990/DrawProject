using System;
using System.Collections.Generic;

namespace SMARTII.Domain.Organization
{
    public class VendorNode : OrganizationNodeBase, IExecutiveOrganizationNode
    {
       
        /// <summary>
        /// 組織深度(高=>低)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 廠商節點
        /// </summary>
        public int? VendorID { get; set; }

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
        public OrganizationType OrganizationType => OrganizationType.Vendor;

        #region reference

        /// <summary>
        /// 組織階層定義檔
        /// </summary>
        public OrganizationNodeDefinition OrganizationNodeDefinitaion { get; set; }

        /// <summary>
        /// 以 廠商來說 , 切分點是統包商
        /// 這邊指的是所屬的 統包商 結點
        /// </summary>
        public VendorNode VendorParent { get; set; }

        /// <summary>
        ///  以 廠商來說 , 切分點是統包商
        /// 這邊指的是 統包商 下的所有結點
        /// </summary>
        public List<VendorNode> VendorChildren { get; set; }

        /// <summary>
        /// 合約對象
        /// 這邊指合約負責單位 : 總部
        /// </summary>
        public List<HeaderQuarterNode> HeaderQuarterNodes { get; set; }

        #endregion reference
    }
}
