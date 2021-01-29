using System.ComponentModel;
using SMARTII.Domain.Common;
using SMARTII.Domain.Security;

namespace SMARTII.Domain.Organization
{
    public class ConcatableUser : OrganizationUser
    {
        public string NotificationBehavior { get; set; }

        public string NotificationKind { get; set; }

        public string NotificationRemark { get; set; }

        [Security]
        [Description("信箱")]
        [Custom("信箱")]
        public string Email { get; set; }

        [Description("手機")]
        [Custom("手機")]
        public string Mobile { get; set; }

        [Security]
        [Description("電話1")]
        [Custom("電話1")]
        public string Telephone { get; set; }

        [Security]
        [Description("電話2")]
        [Custom("電話2")]
        public string TelephoneBak { get; set; }

        [Security]
        [Description("地址")]
        [Custom("地址")]
        public string Address { get; set; }

        [Description("性別")]
        public GenderType? Gender { get; set; }
    }
}
