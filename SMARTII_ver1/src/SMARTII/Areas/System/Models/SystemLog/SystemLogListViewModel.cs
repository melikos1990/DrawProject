using System;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.System.Models.SystemLog
{
    public class SystemLogListViewModel
    {
        public SystemLogListViewModel(Domain.System.SystemLog Log)
        {
            this.Content = Log.Content;
            this.FeatureName = Log.FeatureName;
            this.CreateUserName = Log.CreateUserName.DisplayWhenNull(text: "並無驗證資訊");
            this.CreateDateTime = Log.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Operator = String.Join(",", Log.Operator.ParseEnumFlags());
        }

        public string FeatureName { get; set; }

        public string Content { get; set; }

        public string CreateUserName { get; set; }

        public string CreateDateTime { get; set; }

        public string Operator { get; set; }
    }
}
