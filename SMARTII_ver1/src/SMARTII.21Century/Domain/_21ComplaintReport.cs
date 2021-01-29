using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;

namespace SMARTII._21Century.Domain
{
    public class _21ComplaintReport : BaseComplaintReport
    {
        /// <summary>
        /// 門市名稱
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 反應日期
        /// </summary>
        public DateTime CaseCreateDate { get; set; }
        /// <summary>
        /// 發生日期
        /// </summary>
        public new Nullable<DateTime> CreateDate { get; set; }
        /// <summary>
        /// 填單者
        /// </summary>
        public string WriteUser { get; set; }
        /// <summary>
        /// 0800客服
        /// </summary>
        public bool CallCenter{ get; set; }
        /// <summary>
        /// 21世紀官方網站
        /// </summary>
        public bool OfficalWebSite{ get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public bool Other{ get; set; }


    }
}
