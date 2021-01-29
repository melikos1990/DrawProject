using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.WorkSchedule
{
    public class WorkScheduleErrorViewModel
    {

        public WorkScheduleErrorViewModel(SMARTII.Domain.Master.WorkSchedule domain)
        {
            this.BuName = domain.NodeName;
            this.Date = (domain.Date as DateTime?).DisplayWhenNull("yyyy-MM-dd");
        }

        public string BuName { get; set; }

        public string Date { get; set; }

    }
}
