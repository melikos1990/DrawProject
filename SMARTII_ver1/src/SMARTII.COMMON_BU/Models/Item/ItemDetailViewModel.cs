using System.Dynamic;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.COMMON_BU.Models.Item
{
    public class ItemDetailViewModel
    {
        public ItemDetailViewModel()
        {
        }

        public ItemDetailViewModel(Domain.Master.Item<ExpandoObject> data)
        {
            this.ID = data.ID;
            this.BUName = data.NodeName;
            this.NodeID = data.NodeID;
            this.Code = data.Code;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled;
            this.Description = data.Description;
            this.Particular = data.Particular;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = data.UpdateUserName;
            this.ImagePath = data.ImagePath;
        }

        public Domain.Master.Item ToDomain()
        {
            var result = new Domain.Master.Item()
            {
                ID = this.ID,
                NodeID = this.NodeID,
                Code = this.Code,
                IsEnabled = this.IsEnabled,
                Description = this.Description,
                Name = this.Name,
                OrganizationType = OrganizationType.HeaderQuarter,
                Picture = this.Picture?.ToList()
            };

            return result;
        }

        public string Description { get; set; }

        public int ID { get; set; }

        public int NodeID { get; set; }

        public string BUName { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public string Code { get; set; }

        public ExpandoObject Particular { get; set; }

        public string[] ImagePath { get; set; }

        public HttpFile[] Picture { get; set; }

        public string CreateDateTime { get; set; }

        public string CreateUserName { get; set; }

        public string UpdateDateTime { get; set; }

        public string UpdateUserName { get; set; }

        public string NodeKey { get; set; }
    }
}
