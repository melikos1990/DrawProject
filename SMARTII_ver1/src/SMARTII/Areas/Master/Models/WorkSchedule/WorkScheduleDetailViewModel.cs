using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.WorkSchedule
{
    public class WorkScheduleDetailViewModel
    {
        public WorkScheduleDetailViewModel()
        {
        }

        public WorkScheduleDetailViewModel(Domain.Master.WorkSchedule data)
        {
            this.ID = data.ID;
            this.Title = data.Title;
            this.DateStr = ((DateTime?)data.Date).DisplayWhenNull("yyyy-MM-dd");
            this.WorkType = data.WorkType;
            this.WorkTypeName = data.WorkType.GetDescription();
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull(); ;
            this.CreateDateTime = ((DateTime?)data.CreateDateTime).DisplayWhenNull();
            this.CreateUserName = data.CreateUserName;
            this.OrganizationType = OrganizationType.HeaderQuarter;
            this.BuID = data.NodeID;
            this.BuName = data.NodeName;
        }
        public Domain.Master.WorkSchedule ToDomain()
        {
            var result = new Domain.Master.WorkSchedule();
            result.ID = this.ID;
            result.Title = this.Title;
            result.Date = this.Date.Date;
            result.WorkType = this.WorkType;
            result.NodeID = this.BuID;
            return result;
        }
        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DateStr { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        public WorkType WorkType { get; set; }
        /// <summary>
        /// 類別名稱
        /// </summary>
        public string WorkTypeName { get; set; }
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
        /// 組織型別
        /// </summary>
        public OrganizationType OrganizationType { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int BuID{ get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }

        public DateTime Date {
            get {
                return  Convert.ToDateTime(this.DateStr);
            }
        }
    }
}
