using System;
using Ptc.Data.Condition2.Interface.Type;

namespace SMARTII.Domain.Data
{
    public class PagingRequest
    {
        public PagingRequest()
        {
        }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public string sort { get; set; }

        public string direction { get; set; }

        public OrderType orderType
        {
            get
            {
                return direction?.ToLower() == "asc" ? OrderType.Asc : OrderType.Desc;
            }
        }
    }

    public class PagingRequest<T> : PagingRequest
    {
        public PagingRequest()
        {
        }

        /// <summary>
        /// 自訂的查詢條件
        /// </summary>
        public T criteria { get; set; }
    }
}
