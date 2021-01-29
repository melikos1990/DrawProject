using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentNoticeViewModel : CaseAssignmentBaseViewModel
    {
        public CaseAssignmentNoticeViewModel()
        {
        }

        public CaseAssignmentNoticeViewModel(CaseAssignmentComplaintNotice notice)
        {
            this.CaseAssignmentComplaintNoticeType = notice.CaseAssignmentComplaintNoticeType;
            this.CaseAssignmentComplaintNoticeTypeName = notice.CaseAssignmentComplaintNoticeType.GetDescription();
            this.CaseAssignmentProcessType = notice.CaseAssignmentProcessType;
            this.CaseAssignmentProcessTypeName = notice.CaseAssignmentProcessType.GetDescription();
            this.CaseID = notice.CaseID;
            this.Content = notice.Content;
            this.CreateDateTime = notice.CreateDateTime;
            this.CreateUserName = notice.CreateUserName;
            this.EMLFilePath = notice.EMLFilePath;
            this.FilePath = notice.FilePath;
            this.ID = notice.ID;
            this.NodeID = notice.NodeID;
            this.NotificationUsers = string.Join(",", notice.NoticeUsers ?? new string[] { }); 
            this.NotificationBehaviors = notice.NotificationBehaviors;
            this.NotificationDateTime = notice.NotificationDateTime;
            this.OrganizationType = notice.OrganizationType;
            this.Users = notice.Users?
                               .Select(x => new CaseAssignmentNoticeUserViewModel(x))
                               .ToList();

         

        }
        public CaseAssignmentComplaintNotice ToDomain()
        {
            var user = new CaseAssignmentComplaintNotice();
            user.CaseAssignmentComplaintNoticeType = this.CaseAssignmentComplaintNoticeType;
            user.CaseAssignmentProcessType = this.CaseAssignmentProcessType;
            user.CaseID = this.CaseID;
            user.Content = this.Content;
            user.CreateDateTime = this.CreateDateTime;
            user.CreateUserName = this.CreateUserName;
            user.Files = this.Files;
            user.ID = this.ID;
            user.NodeID = this.NodeID;
            user.NoticeUsers = this.NotificationUsers?.Split(',');
            user.NotificationBehaviors = this.NotificationBehaviors;
            user.NotificationDateTime = this.NotificationDateTime;
            user.OrganizationType = this.OrganizationType;
            user.Users = this.Users?
                               .Select(x => x.ToDomain())
                               .ToList();
            return user;
        }


        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 通知對象
        /// </summary>
        public List<CaseAssignmentNoticeUserViewModel> Users { get; set; }
        /// <summary>
        /// 通知狀態
        /// ※目前就業為行為來說 , 通知發出就是已通知
        /// 因此並未針對該欄位進行寫入DB
        /// </summary>
        public CaseAssignmentComplaintNoticeType CaseAssignmentComplaintNoticeType { get; set; }

        /// <summary>
        /// 通知狀態名稱
        /// </summary>
        public string CaseAssignmentComplaintNoticeTypeName { get; set; }

    }
}
