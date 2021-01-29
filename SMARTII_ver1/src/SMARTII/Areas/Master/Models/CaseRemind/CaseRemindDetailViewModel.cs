using System;
using System.Collections.Generic;
using System.Linq;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.CaseRemind
{
    public class CaseRemindDetailViewModel
    {
        public CaseRemindDetailViewModel()
        {
        }

        public Domain.Case.CaseRemind ToDomain()
        {
            var result = new Domain.Case.CaseRemind();
            result.ID = this.ID;
            result.CaseID = this.CaseID;
            result.AssignmentID = this.AssignmentID;
            result.OrganizationType = OrganizationType.HeaderQuarter;
            result.ActiveEndDateTime = this.ActiveEndDateTime.Value;
            result.ActiveStartDateTime = this.ActiveStartDateTime.Value;
            result.Content = this.Content;
            result.UserIDs = this.Users?.Select(x => x.UserID)?.ToList() ?? new List<string>();
            result.IsConfirm = this.IsConfirm;
            result.Type = this.Level;
            return result;
        }

        public CaseRemindDetailViewModel(Domain.Case.CaseRemind data)
        {
            this.ID = data.ID;
            this.CaseID = data.CaseID;
            this.AssignmentID = data.AssignmentID;
            this.Content = data.Content;
            this.IsConfirm = data.IsConfirm;
            this.Level = data.Type;
            this.ConfirmUserID = data.ConfirmUserID;
            this.ConfirmDateTime = data.ConfirmDateTime.DisplayWhenNull();
            this.ConfirmUserName = data.ConfirmUserName.DisplayWhenNull();
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;

            this.ActiveDateTimeRange = StringUtility.ToDateRangePicker(data.ActiveStartDateTime, data.ActiveEndDateTime);
            this.UserIDs = data.Users.Select(x => x.UserID).ToArray();
            this.Users = data.Users
                             .Select(x => new UserListViewModel(x))
                             .ToList();
            this.IsLock = (DateTime.Compare(DateTime.Now, data.ActiveStartDateTime) > 0) ? true : false ;
        }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 轉派編號
        /// </summary>
        public int? AssignmentID { get; set; }

        /// <summary>
        /// 通知內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否確認完成
        /// </summary>
        public bool IsConfirm { get; set; }

        /// <summary>
        /// 通知等級
        /// </summary>
        public CaseRemindType Level { get; set; }

        /// <summary>
        /// 通知對象
        /// </summary>
        public string[] UserIDs { get; set; } = new string[] { };

        public List<UserListViewModel> Users { get; set; }

        /// <summary>
        /// 完成人員代號
        /// </summary>
        public string ConfirmUserID { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public string ConfirmDateTime { get; set; }

        /// <summary>
        /// 完成人員
        /// </summary>
        public string ConfirmUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 生效區間
        /// </summary>
        public string ActiveDateTimeRange { get; set; }

        /// <summary>
        /// 開始生效日
        /// </summary>
        public DateTime? ActiveStartDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ActiveDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ActiveDateTimeRange.Split('-')[0].Trim());
            }
        }

        /// <summary>
        /// 結束生效日
        /// </summary>
        public DateTime? ActiveEndDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ActiveDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ActiveDateTimeRange.Split('-')[1].Trim());
            }
        }

        /// <summary>
        /// 是否鎖定編輯頁面
        /// </summary>
        public bool IsLock { get; set; }
    }
}
