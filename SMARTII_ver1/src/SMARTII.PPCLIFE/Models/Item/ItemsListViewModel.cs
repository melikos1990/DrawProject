using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;

namespace SMARTII.PPCLIFE.Models.Item
{
    public class ItemsListViewModel
    {

        public ItemsListViewModel() { }


        public ItemsListViewModel(SMARTII.Domain.Master.Item item)
        {
            this.BUName = item.NodeName;
            this.Name = item.Name;
            this.Description = item.Description;
            this.IsEnabled = item.IsEnabled.DisplayBit(@true: "啟用", @false: "停用"); ;
            this.ID = item.ID;
        }


        public string BUName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IsEnabled { get; set; }

        public int ID { get; set; }

    }
}
