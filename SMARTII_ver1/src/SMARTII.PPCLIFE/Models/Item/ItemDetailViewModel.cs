using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Models.Item
{
    public class ItemDetailViewModel : COMMON_BU.Models.Item.ItemDetailViewModel
    {
        public ItemDetailViewModel()
        {
        }

        public ItemDetailViewModel(Item<PPCLIFE_Item> data)
        {
            this.ID = data.ID;
            this.NodeID = data.NodeID;
            this.Name = data.Name;
            this.Code = data.Code;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;

            if (data.Particular != null)
            {
                this.Particular.Channel = data.Particular.Channel;
                this.Particular.InternationalBarcode = data.Particular.InternationalBarcode;
                this.Particular.CanReturn = data.Particular.CanReturn;
                this.Particular.StopSalesDateTime = data.Particular.StopSalesDateTime;
            }
        }

        public new Item<PPCLIFE_Item> ToDomain()
        {
            var result = new Item<PPCLIFE_Item>()
            {
                ID = this.ID,
                NodeID = this.NodeID,
                Code = this.Code,
                IsEnabled = this.IsEnabled,
                Description = this.Description,
                Name = this.Name,
                OrganizationType = OrganizationType.HeaderQuarter,
                Particular = this.Particular ?? new PPCLIFE_Item()
                {
                    Channel = null,
                    InternationalBarcode = string.Empty,
                    CanReturn = false,
                    StopSalesDateTime = null,
                },
                Picture = this.Picture?.ToList()
            };

            return result;
        }

        public PPCLIFE_Item Particular { get; set; }
    }
}
