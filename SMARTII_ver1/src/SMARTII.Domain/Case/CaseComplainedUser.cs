using System.ComponentModel;
using System.Security.Permissions;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Security;
using SecurityAttribute = SMARTII.Domain.Security.SecurityAttribute;

namespace SMARTII.Domain.Case
{
    /// <summary>
    /// 被反應對象
    /// </summary>
    public class CaseComplainedUser : ConcatableUser ,IStoreRelationship
    {
        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件代號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 權責型態 (知會單位/權責單位)
        /// </summary>
        [Description("權責型態")]
        public CaseComplainedUserType CaseComplainedUserType { get; set; }

        /// <summary>
        /// 所屬的案件
        /// </summary>
        public Case Case { get; set; }

        /// <summary>
        /// 負責人姓名
        /// </summary>
        [Security]
        [Description("負責人姓名")]
        public string OwnerUserName { get; set; }

        /// <summary>
        /// 負責人電話
        /// </summary>
        [Security]
        [Description("負責人電話")]
        public string OwnerUserPhone { get; set; }

        /// <summary>
        /// 負責人職稱
        /// </summary>
        [Description("負責人職稱")]
        public string OwnerJobName { get; set; }

        /// <summary>
        /// 負責人Email
        /// </summary>
        [Description("負責人信箱")]
        public string OwnerUserEmail { get; set; }

        /// <summary>
        /// 區經理姓名
        /// </summary>
        [Security]
        [Description("區經理姓名")]
        public string SupervisorUserName { get; set; }

        /// <summary>
        /// 區經理電話
        /// </summary>
        [Security]
        [Description("區經理電話")]
        public string SupervisorUserPhone { get; set; }

        /// <summary>
        /// 區經理職稱
        /// </summary>
        [Description("區經理職稱")]
        public string SupervisorJobName { get; set; }


        /// <summary>
        /// 區經理Email
        /// </summary>
        [Description("區經理信箱")]
        public string SupervisorUserEmail { get; set; }

        /// <summary>
        /// 區組管組織節點
        /// </summary>
        [Description("區組管組織節點")]
        public string SupervisorNodeName { get; set; }
        /// <summary>
        /// 門市型態
        /// </summary>
        public int StoreType { get; set; }
        /// <summary>
        /// 門市型態
        /// </summary>
        public string StoreTypeName { get; set; }
    }
}
