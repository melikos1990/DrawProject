using System.Dynamic;
using SMARTII.Domain.Data;

namespace SMARTII.COMMON_BU.Models.Store
{
    public class StoreListViewModel
    {
        public StoreListViewModel()
        {
        }

        public StoreListViewModel(Domain.Organization.Store<ExpandoObject> data)
        {
            this.BuID = data.BuID;
            this.NodeName = data.BuName;
            this.NodeID = data.NodeID;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled.DisplayBit(@true: "啟用", @false: "停用");
            this.Code = data.Code;
            this.Name = data.Name;
            this.Address = data.Address;
            this.Telephone = data.Telephone;
            this.StoreOpenDateTime = data.StoreOpenDateTime?.ToString("yyyy/MM/dd HH:mm:ss");
            this.StoreCloseDateTime = data.StoreCloseDateTime?.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public int? BuID { get; set; }

        public string NodeName { get; set; }

        public int NodeID { get; set; }

        public string Name { get; set; }

        public string IsEnabled { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string StoreOpenDateTime { get; set; }

        public string StoreCloseDateTime { get; set; }

    }
}
