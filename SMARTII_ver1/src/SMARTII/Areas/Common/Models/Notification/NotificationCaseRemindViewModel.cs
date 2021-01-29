using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Common.Models.Notification
{
    public class NotificationCaseRemindViewModel: NotificationBaseViewModel
    {

        public NotificationCaseRemindViewModel(CaseRemind caseRemind)
        {
            this.ID = caseRemind.ID.ToString();
            this.Title = $"{caseRemind.NodeName}案件追蹤";
            this.Content = caseRemind.Content;
            this.BuName = caseRemind.NodeName;
            this.CaseID = caseRemind.CaseID;
            this.AssignmentID = caseRemind.AssignmentID;
            this.Level = caseRemind.Type;
            this.LevelName = caseRemind.Type.GetDescription();
            this.ActiveStartDateTime = caseRemind.ActiveStartDateTime.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 生效時間 開始
        /// </summary>
        public string ActiveStartDateTime { get; set; }
        

        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        
        /// <summary>
        /// 案件編號
        /// </summary>
        public int? AssignmentID { get; set; }

        /// <summary>
        /// 緊急等級
        /// </summary>
        public CaseRemindType Level { get; set; }

        /// <summary>
        /// 緊急等級 名稱
        /// </summary>
        public string LevelName { get; set; }


        new public PersonalNotificationType PersonalNotificationType { get; set; } = PersonalNotificationType.CaseRemind;

    }
}
