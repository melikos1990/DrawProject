using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;

namespace SMARTII.Areas.Master.Models.Billboard
{
    public class BillboardListViewModel
    {
        public BillboardListViewModel()
        {
        }

        public BillboardListViewModel(Domain.Master.Billboard data)
        {
            this.ID = data.ID;
            this.Title = data.Title;
            this.Content = data.Content;
            this.BillboardWarningType = data.BillboardWarningType;
            this.BillboardWarningTypeName = data.BillboardWarningType.GetDescription();
            this.CreateUserName = data.CreateUserName;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.ActiveStartDateTime = data.ActiveStartDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.ActiveEndDateTime = data.ActiveEndDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.ActiveDateTimeRange = StringUtility.ToDateRangePicker(data.ActiveStartDateTime, data.ActiveEndDateTime);
            this.FilePaths = data.FilePaths;
            this.ImagePath = data.ImagePath;
            this.Name = data.CreateUserName;
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public BillboardWarningType BillboardWarningType { get; set; }

        public string BillboardWarningTypeName { get; set; }

        public string CreateUserName { get; set; }

        public string CreateDateTime { get; set; }

        public string ActiveDateTimeRange { get; set; }

        public string ActiveStartDateTime { get; set; }

        public string ActiveEndDateTime { get; set; }

        public string[] FilePaths { get; set; }

        public PersonalNotificationType PersonalNotificationType { get; set; } = PersonalNotificationType.Billboard;

        public string ImagePath { get; set; }

        public string Name { get; set; }
    }
}
