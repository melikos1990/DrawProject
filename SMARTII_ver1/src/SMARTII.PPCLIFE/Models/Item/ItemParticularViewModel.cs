using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Master;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Models.Item
{
    public class ItemParticularViewModel
    {

        public ItemParticularViewModel() { }

        public ItemParticularViewModel(Domain.PPCLIFE_Item domain)
        {
            this.CanReturn = domain.CanReturn;
            this.InternationalBarcode = domain.InternationalBarcode;
            this.Channel = domain.Channel;
            this.StopSalesDateTime = domain.StopSalesDateTime;
        }
        
        [MSSQLFilter("x => x.Particular.Channel != null && x.Particular.Channel.Contains(Value)",
        PredicateType.And)]
        public string Channel { get; set; }

        
        [MSSQLFilter("x => x.Particular.InternationalBarcode != null && x.Particular.InternationalBarcode.Contains(Value)",
        PredicateType.And)]
        public string InternationalBarcode { get; set; }


        [MSSQLFilter("x => x.Particular.CanReturn != null && x.Particular.CanReturn == Value",
        PredicateType.And)]
        public Boolean CanReturn { get; set; }


        [MSSQLFilter("x => x.Particular.StopSalesDateTime != null && x.Particular.StopSalesDateTime <= Value",
        PredicateType.And)]
        public DateTime? StopSalesDateTime { get; set; }

    }
}
