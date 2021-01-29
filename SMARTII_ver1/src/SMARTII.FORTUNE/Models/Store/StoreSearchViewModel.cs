using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.FORTUNE.Domain;

namespace SMARTII.FORTUNE.Models.Store
{
    public class StoreSearchViewModel : COMMON_BU.Models.Store.StoreSearchViewModel
    {
        public StoreSearchViewModel()
        {
        }

        public StoreParticularViewModel Particular { get; set; }
    }
}
