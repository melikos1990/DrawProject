using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Organization.Models.User
{
    public class HeaderQuarterUserListViewModel : UserListViewModel
    {
        public HeaderQuarterUserListViewModel(Domain.Organization.User user) : base(user)
        {
        }

        public HeaderQuarterUserListViewModel(Domain.Organization.User user, Domain.Organization.HeaderQuarterNode node) : base(user, node)
        {
            this.BUName = node.BusinessParent?.Name;
            this.BUID = node.BusinessParent?.NodeID;
        }

        /// <summary>
        /// 企業代號
        /// </summary>
        public int? BUID { get; set; }

        /// <summary>
        /// 企業名稱
        /// </summary>
        public string BUName { get; set; }
    }

    public class CallCenterUserListViewModel : UserListViewModel
    {
        public CallCenterUserListViewModel(Domain.Organization.User user) : base(user)
        {
        }

        public CallCenterUserListViewModel(Domain.Organization.User user, Domain.Organization.CallCenterNode node) : base(user, node)
        {
            this.CallCenterName = node.CallCenterParent?.Name;
            this.CallCenterID = node.CallCenterParent?.NodeID;
        }

        /// <summary>
        /// 客服中心代號
        /// </summary>
        public int? CallCenterID { get; set; }

        /// <summary>
        /// 客服中心代號名稱
        /// </summary>
        public string CallCenterName { get; set; }
    }

    public class VendorUserListViewModel : UserListViewModel
    {
        public VendorUserListViewModel(Domain.Organization.User user) : base(user)
        {
        }

        public VendorUserListViewModel(Domain.Organization.User user, Domain.Organization.VendorNode node) : base(user, node)
        {
            this.VendorName = node.VendorParent?.Name;
            this.VendorID = node.VendorParent?.NodeID;
        }

        /// <summary>
        /// 廠商代號
        /// </summary>
        public int? VendorID { get; set; }

        /// <summary>
        /// 廠商名稱
        /// </summary>
        public string VendorName { get; set; }
    }

    public class UserListViewModel
    {
        public UserListViewModel()
        {
        }

        public UserListViewModel(Domain.Organization.User user)
        {
            this.UserID = user.UserID;
            this.IsEnabled = user.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
            this.IsAD = user.IsAD.DisplayBit();
            this.IsSystemUser = user.IsSystemUser.DisplayBit();
            this.UserName = user.Name;
            this.Account = user.Account;
            this.CreateUserName = user.CreateUserName;
            this.CreateDateTime = user.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.RoleNames = user.Roles?.Select(x => x.Name).ToArray();
            this.Email = user.Email;
            this.Mobile = user.Mobile;
            this.Telephone = user.Telephone;
            this.Address = user.Address;
        }

        public UserListViewModel(Domain.Organization.User user, IOrganizationNode node) : this(user)
        {
            var position = user.JobPositions?
                               .FirstOrDefault(x => x.NodeID == node.NodeID &&
                                                    x.OrganizationType == node.OrganizationType);

            this.JobName = position?.Job?.Name;
            this.JobID = position?.Job?.ID;
            this.Level = position?.Job?.Level;
            this.NodeID = node.NodeID;
            this.NodeName = node.Name;
            this.OrganizationType = node.OrganizationType;
            this.OrganizationTypeName = node.OrganizationType.GetDescription();
        }

        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsEnabled { get; set; }

        /// <summary>
        /// 是否為內部登入人員
        /// </summary>
        public string IsAD { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 節點代號
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 節點名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 組織型態名稱
        /// </summary>
        public string OrganizationTypeName { get; set; }

        /// <summary>
        /// 職稱代號
        /// </summary>
        public int? JobID { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 組織職稱定義
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 角色清單
        /// </summary>
        public string[] RoleNames { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 系統使用者
        /// </summary>
        public string IsSystemUser { get; set; }
    }
}
