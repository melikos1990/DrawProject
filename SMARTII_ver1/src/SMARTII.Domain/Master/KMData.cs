using System;
using System.Collections.Generic;
using MultipartDataMediaFormatter.Infrastructure;

namespace SMARTII.Domain.Master
{
    public class KMData
    {
        public KMData()
        {
        }

        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 分類代號
        /// </summary>
        public int ClassificationID { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 所屬的常見問題分類
        /// </summary>
        public KMClassification KMClassification { get; set; }

        /// <summary>
        /// 黨案路徑
        /// </summary>
        public List<string> FilePaths { get; set; }

        /// <summary>
        /// 實體檔案
        /// </summary>
        public List<HttpFile> Files { get; set; }
    }
}
