using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMARTII.COMMON_BU
{
    public class ImportStoreData
    {
        /// <summary>
        /// 上層組織節點
        /// </summary>
        public string UpperOrganizationNodeName { get; set; }
        /// <summary>
        /// 節點名稱
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 門市店號
        /// </summary>
        public string StoreNo { get; set; }
        /// <summary>
        /// 門市名稱
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 職稱
        /// </summary>
        public string[] JobTitle { get; set; }
        /// <summary>
        /// 職稱ID
        /// </summary>
        public int[] JobID { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// NodeID
        /// </summary>
        public int NodeID { get; set; }
    }
}
