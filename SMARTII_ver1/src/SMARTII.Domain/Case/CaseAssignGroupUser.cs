using SMARTII.Domain.Common;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Case
{
    public class CaseAssignGroupUser : OrganizationUser, IUserRelationship
    {
        public CaseAssignGroupUser()
        {
        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 群組代號
        /// </summary>
        public int GroupID { get; set; }

        public CaseAssignGroup CaseAssignGroup { get; set; }

        /// <summary>
        /// 地址(加密)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 提醒方式 (EX : Mobile)
        /// </summary>
        public string NotificationBehavior { get; set; }

        /// <summary>
        /// 提醒註記 (EX : CC/收件者/密件)
        /// </summary>   
        public string NotificationRemark { get; set; }

        /// <summary>
        /// 提醒種類 (EX : jpush)
        /// </summary>
        public string NotificationKind { get; set; }

        /// <summary>
        /// 市話(加密)
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 信箱(加密)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public GenderType? Gender { get; set; }



        public User User { get; set; }
    }
}
