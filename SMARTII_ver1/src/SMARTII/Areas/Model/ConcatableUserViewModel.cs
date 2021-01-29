using SMARTII.Domain.Common;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Model
{
    public class ConcatableUserViewModel : OrganizationUserViewModel
    {
        public ConcatableUserViewModel()
        {
        }

        public string NotificationBehavior { get; set; }

        public string NotificationKind { get; set; }

        public string NotificationRemark { get; set; }
        public string Email { get; set; }

        public string Mobile { get; set; }
        public string Telephone { get; set; }

        public string TelephoneBak { get; set; }

        public string Address { get; set; }

        public GenderType? Gender { get; set; }

        public string GenderTypeName { get; set; }
    }
}
