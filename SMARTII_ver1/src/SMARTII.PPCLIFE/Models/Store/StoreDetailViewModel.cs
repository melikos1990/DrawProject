using System;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Models.Store
{
    public class StoreDetailViewModel : COMMON_BU.Models.Store.StoreDetailViewModel
    {
        public StoreDetailViewModel()
        {
        }

        public StoreDetailViewModel(Store<PPCLIFE_Store> data)
        {
            this.NodeID = data.NodeID;
            this.Name = data.Name;
            this.Code = data.Code;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;

            if (data.Particular != null)
            {
                this.Particular.CreditCard = data.Particular.CreditCard;
                this.Particular.MobilePay = data.Particular.MobilePay;
                this.Particular.Park = data.Particular.Park;
                this.Particular.OrderLimit = data.Particular.OrderLimit;
                this.Particular.Toilet = data.Particular.Toilet;
                this.Particular.NumberTable = data.Particular.NumberTable;
                this.Particular.Delivery = data.Particular.Delivery;
                this.Particular.PurchaseDate = data.Particular.PurchaseDate;
            }
        }

        public new Store<PPCLIFE_Store> ToDomain()
        {
            var result = new Store<PPCLIFE_Store>()
            {
                NodeID = this.NodeID,
                Code = this.Code,
                IsEnabled = this.IsEnabled,
                Name = this.Name,
                Address = this.Address,
                ServiceTime = this.ServiceTime,
                Email = this.Email,
                Telephone = this.Telephone,
                Memo = this.Memo,
                StoreType = this.StoreType,
                SupervisorNodeJobID = this.SupervisorNodeJobID,
                OwnerNodeJobID = this.OwnerNodeJobID,
                StoreCloseDateTime = string.IsNullOrEmpty(this.StoreCloseDateTime) ? default(DateTime?) : Convert.ToDateTime(this.StoreCloseDateTime),
                StoreOpenDateTime = string.IsNullOrEmpty(this.StoreOpenDateTime) ? default(DateTime?) : Convert.ToDateTime(this.StoreOpenDateTime),
                OrganizationType = OrganizationType.HeaderQuarter,
                Particular = this.Particular,
            };

            return result;
        }

        public new PPCLIFE_Store Particular { get; set; }
    }
}
