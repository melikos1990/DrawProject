using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Models.Item
{
    public class ItemSearchViewModel
    {
        public ItemSearchViewModel()
        {
        }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 管理貨號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 企業別代號
        /// </summary>
        public int? NodeID { get; set; }

        public ItemParticularViewModel Particular { get; set; }
    }
}
