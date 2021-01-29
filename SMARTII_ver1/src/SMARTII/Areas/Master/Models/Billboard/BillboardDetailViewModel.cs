using System;
using System.Collections.Generic;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Areas.Organization.Models.User;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;

namespace SMARTII.Areas.Master.Models.Billboard
{
    public class BillboardDetailViewModel
    {
        public BillboardDetailViewModel()
        {
        }

        public BillboardDetailViewModel(Domain.Master.Billboard data)
        {
            this.ID = data.ID;
            this.Title = data.Title;
            this.Content = data.Content;
            this.FilePaths = data.FilePaths;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;
            this.BillboardWarningType = data.BillboardWarningType;
            this.ActiveDateTimeRange = StringUtility.ToDateRangePicker(data.ActiveStartDateTime, data.ActiveEndDateTime);
            this.UserIDs = data.UserIDs?.ToArray();
            this.Users = data.Users
                             .Select(x => new UserListViewModel(x))
                             .ToList();
        }

        public Domain.Master.Billboard ToDomain()
        {
            var domain = new Domain.Master.Billboard();

            domain.ID = this.ID;
            domain.Title = this.Title;
            domain.Content = this.Content;
            domain.ActiveEndDateTime = this.ActiveEndDateTime.Value;
            domain.ActiveStartDateTime = this.ActiveStartDateTime.Value;
            domain.BillboardWarningType = this.BillboardWarningType;
            domain.UserIDs = this.Users?.Select(x => x.UserID)?.ToList() ?? new List<string>();
            domain.Files = this.Files?.ToList();

            ///欄位為檔案存檔位址 , 因此需解析回物件中
            ///組回檔案路徑中，因此與其他主檔規則不同
            domain.CreateDateTime = this.CreateDateTime != null ? DateTime.Parse(this.CreateDateTime) : DateTime.Now;

            return domain;
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public HttpFile[] Files { get; set; }

        public string[] FilePaths { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public string UpdateDateTime { get; set; }

        public string UpdateUserName { get; set; }

        public BillboardWarningType BillboardWarningType { get; set; }

        public string ActiveDateTimeRange { get; set; }

        public DateTime? ActiveStartDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ActiveDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ActiveDateTimeRange.Split('-')[0].Trim());
            }
        }

        public DateTime? ActiveEndDateTime
        {
            get
            {
                return String.IsNullOrEmpty(this.ActiveDateTimeRange) ?
                    default(DateTime?) :
                    Convert.ToDateTime(this.ActiveDateTimeRange.Split('-')[1].Trim());
            }
        }

        public string[] UserIDs { get; set; } = new string[] { };

        public List<UserListViewModel> Users { get; set; }
    }
}
