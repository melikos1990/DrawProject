using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;

namespace SMARTII.Areas.Master.Models.WorkSchedule
{
    public class WorkScheduleListViewModel
    {
        public WorkScheduleListViewModel()
        { }
        public WorkScheduleListViewModel(Domain.Master.WorkSchedule data)
        {
            this.ID = data.ID;
            this.NodeID = data.NodeID;
            this.NodeName = data.NodeName;
            this.Date = ((DateTime?)data.Date).DisplayWhenNull("yyyy-MM-dd");
            this.Title = data.Title;
            this.WorkType = data.WorkType.GetDescription();
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull("yyyy-MM-dd hh:mm");
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
        }
        /// <summary>
        /// 代號
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 類別
        /// </summary>
        public string WorkType { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }
        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
