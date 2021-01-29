using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.PPCLIFE.Models.Store
{
    public class StoreParticularViewModel
    {

        [MSSQLFilter("x => x.Particular.CreditCard != null && x.Particular.CreditCard == Value",
        PredicateType.And)]
        public Boolean? CreditCard { get; set; }

        [MSSQLFilter("x => x.Particular.MobilePay != null && x.Particular.MobilePay.Contains(Value)",
        PredicateType.And)]
        public string MobilePay { get; set; }

        [MSSQLFilter("x => x.Particular.Park != null && x.Particular.Park == Value",
        PredicateType.And)]
        public Boolean? Park { get; set; }

        [MSSQLFilter("x => x.Particular.OrderLimit != null && x.Particular.OrderLimit.Contains(Value)",
        PredicateType.And)]
        public string OrderLimit { get; set; }

        [MSSQLFilter("x => x.Particular.Toilet != null && x.Particular.Toilet == Value",
        PredicateType.And)]
        public Boolean? Toilet { get; set; }

        [MSSQLFilter("x => x.Particular.NumberTable != null && x.Particular.NumberTable.Contains(Value)",
        PredicateType.And)]
        public string NumberTable { get; set; }

        [MSSQLFilter("x => x.Particular.Delivery != null && x.Particular.Delivery == Value",
        PredicateType.And)]
        public Boolean? Delivery { get; set; }

        [MSSQLFilter("x => x.Particular.PurchaseDate != null && x.Particular.PurchaseDate.Contains(Value)",
        PredicateType.And)]
        public string PurchaseDate { get; set; }
    }
}
