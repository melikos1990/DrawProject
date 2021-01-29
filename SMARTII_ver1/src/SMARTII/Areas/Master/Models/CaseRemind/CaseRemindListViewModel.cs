using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Master.Models.CaseRemind
{
    public class CaseRemindListViewModel
    {
        public CaseRemindListViewModel(Domain.Case.CaseRemind data)
        {
            this.ID = data.ID;
            this.ActiveDateTimeRange = StringUtility.ToDateRangePicker(data.ActiveStartDateTime, data.ActiveEndDateTime);
            this.BuID = data.NodeID;
            this.BuName = data.NodeName;
            this.CaseID = data.CaseID;
            this.IsConfirm = data.IsConfirm ? "已完成" : "未完成";
            this.AssignmentID = data.AssignmentID.ToString();
            this.Level = data.Type;
            this.Content = data.Content;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.ActiveStartDateTime = data.ActiveStartDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.ActiveEndDateTime = data.ActiveEndDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.LevelName = data.Type.GetDescription();
        }

        /// <summary>
        /// 生效區間
        /// </summary>
        public string ActiveDateTimeRange { get; set; }

        /// <summary>
        /// 生效時間 結束
        /// </summary>
        public string ActiveEndDateTime { get; set; }

        /// <summary>
        /// 生效時間 開始
        /// </summary>
        public string ActiveStartDateTime { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否確認
        /// </summary>
        public string IsConfirm { get; set; }

        /// <summary>
        /// 案件編號
        /// </summary>
        public string AssignmentID { get; set; }

        /// <summary>
        /// 緊急等級
        /// </summary>
        public CaseRemindType Level { get; set; }

        /// <summary>
        /// 緊急等級 名稱
        /// </summary>
        public string LevelName { get; set; }

        public PersonalNotificationType PersonalNotificationType { get; set; } = PersonalNotificationType.CaseRemind;

    }
}
