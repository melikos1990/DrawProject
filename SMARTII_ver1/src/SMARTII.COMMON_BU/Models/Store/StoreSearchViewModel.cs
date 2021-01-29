using System;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.COMMON_BU.Models.Store
{
    public class StoreSearchViewModel
    {
        public StoreSearchViewModel()
        {
        }
        [MSSQLFilter("HEADQUARTERS_NODE.BU_ID",
        ExpressionType.Equal, 
        PredicateType.And)]
        public int? BuID { get; set; }

        [MSSQLFilter(nameof(STORE.STORE_CLOSE_DATETIME),
        ExpressionType.LessThanOrEqual,
        PredicateType.And
        )]
        public DateTime? StoreCloseDateTime { get; set; }

        [MSSQLFilter("HEADQUARTERS_NODE.LEFT_BOUNDARY",
        ExpressionType.GreaterThanOrEqual,
        PredicateType.And
        )]
        public int? LeftBoundary { get; set; }

        [MSSQLFilter(nameof(STORE.NAME),
        ExpressionType.Parameter,
        PredicateType.And
        )]
        public string Name { get; set; }

        public int? NodeID { get; set; }


        [MSSQLFilter(nameof(STORE.STORE_OPEN_DATETIME),
        ExpressionType.GreaterThanOrEqual,
        PredicateType.And
        )]
        public DateTime? StoreOpenDateTime { get; set; }

        [MSSQLFilter("HEADQUARTERS_NODE.RIGHT_BOUNDARY",
        ExpressionType.LessThanOrEqual,
        PredicateType.And
        )]
        public int? RightBoundary { get; set; }

        public bool IsEnable { get; set; }


    }
}
