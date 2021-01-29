using System.Dynamic;
using SMARTII.Domain.Data;

namespace SMARTII.COMMON_BU.Models.Item
{
    public class ItemListViewModel
    {
        public ItemListViewModel()
        {
        }

        public ItemListViewModel(Domain.Master.Item<ExpandoObject> data)
        {
            this.ID = data.ID;
            this.BUName = data.NodeName;
            this.NodeID = data.NodeID;
            this.Code = data.Code;
            this.Name = data.Name;
            this.Description = data.Description;
            this.Particular = data.Particular;
            this.IsEnabled = data.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
        }

        public string Description { get; set; }

        public int ID { get; set; }

        public int? NodeID { get; set; }

        public string BUName { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string IsEnabled { get; set; }

        public ExpandoObject Particular { get; set; }
    }
}
