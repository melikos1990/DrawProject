using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMARTII.COMMON_BU
{
    public class ImportStorePersonnelData
    {
        /// <summary>
        /// 門市NodeID
        /// </summary>
        public int NodeID { get; set; }
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
        public int[] NodeJobID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 使用者ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 分機
        /// </summary>
        public string EXT { get; set; }
        /// <summary>
        /// 手機
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }
    }
}
