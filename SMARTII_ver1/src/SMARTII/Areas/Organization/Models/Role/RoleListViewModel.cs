using SMARTII.Domain.Data;

namespace SMARTII.Areas.Organization.Models.Role
{
    public class RoleListViewModel
    {
        public RoleListViewModel(Domain.Organization.Role role)
        {
            this.RoleID = role.ID;
            this.RoleName = role.Name;
            this.IsEnabled = role.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
            this.CreateUserName = role.CreateUserName;
            this.CreateDateTime = role.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 操作權限代號
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 操作權限名稱
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public string IsEnabled { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
    }
}
