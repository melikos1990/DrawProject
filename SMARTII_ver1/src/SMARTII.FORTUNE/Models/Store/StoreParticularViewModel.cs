using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;

namespace SMARTII.FORTUNE.Models.Store
{
    public class StoreParticularViewModel
    {

        [MSSQLFilter("x => x.Particular.ChargegunType != null && x.Particular.ChargegunType.Contains(Value)",
        PredicateType.And)]
        public string ChargegunType { get; set; }

        [MSSQLFilter("x => x.Particular.ChargeType != null && x.Particular.ChargeType.Contains(Value)",
        PredicateType.And)]
        public string ChargeType { get; set; }

        [MSSQLFilter("x => x.Particular.MaxPower != null && x.Particular.MaxPower.Contains(Value)",
        PredicateType.And)]
        public string MaxPower { get; set; }

        [MSSQLFilter("x => x.Particular.IsParkingFee != null && x.Particular.IsParkingFee.Contains(Value)",
        PredicateType.And)]
        public string IsParkingFee { get; set; }

        [MSSQLFilter("x => x.Particular.ParkingFee != null && x.Particular.ParkingFee.Contains(Value)",
        PredicateType.And)]
        public string ParkingFee { get; set; }

        [MSSQLFilter("x => x.Particular.CharingFee != null && x.Particular.CharingFee.Contains(Value)",
        PredicateType.And)]
        public string CharingFee { get; set; }
    }
}
