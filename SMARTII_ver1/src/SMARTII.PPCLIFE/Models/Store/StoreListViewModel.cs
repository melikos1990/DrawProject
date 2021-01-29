using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Models.Store
{
    public class StoreListViewModel: COMMON_BU.Models.Store.StoreListViewModel
    {

        public StoreListViewModel() { }

        public StoreListViewModel(SMARTII.Domain.Organization.Store<PPCLIFE_Store> data)
        {
            this.BuID = data.BuID;
            this.NodeName = data.BuName;
            this.NodeID = data.NodeID;
            this.Name = data.Name;
            this.IsEnabled = data.IsEnabled.DisplayBit();
            this.Code = data.Code;
            this.Name = data.Name;
            this.Address = data.Address;
            this.Telephone = data.Telephone;
            this.StoreOpenDateTime = data.StoreOpenDateTime?.ToString("yyyy/MM/dd HH:mm:ss");
            this.StoreCloseDateTime = data.StoreCloseDateTime?.ToString("yyyy/MM/dd HH:mm:ss");
        }


    }
}
