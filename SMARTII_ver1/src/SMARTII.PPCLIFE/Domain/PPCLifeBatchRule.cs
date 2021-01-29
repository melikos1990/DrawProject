using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMARTII.PPCLIFE.Domain
{
    public class PPCLifeBatchRule
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string BatchNo { get; set; }
    }
}
