using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.KMClassification
{
    public class KMDetailViewModel
    {
        public KMDetailViewModel()
        {
        }
       
        public KMDetailViewModel(Domain.Master.KMData data, string BuName)
        {
            this.ID = data.ID;
            this.ClassificationID = data.ClassificationID;
            this.ClassificationName = BuName + "-" + string.Join("-", data.KMClassification.ParentNamePathByArray.Reverse());
            this.Title = data.Title;
            this.Content = data.Content;    
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull(); ;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.FilePaths = data.FilePaths?.ToArray();
        }
        public Domain.Master.KMData ToDomain()
        {
            var result = new Domain.Master.KMData();
            result.ID = this.ID;
            result.ClassificationID = this.ClassificationID;
            result.Title = this.Title;
            result.Content = this.Content;
            result.Files = this.Files?.ToList();
        
            return result;
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
        /// 分類名稱
        /// </summary>
        public string ClassificationName { get; set; }
        /// <summary>
        /// 主旨
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }     
        /// <summary>
        /// 副檔路徑
        /// </summary>
        public string[] FilePaths { get; set; }
        /// <summary>
        /// 實體檔案
        /// </summary>
        public HttpFile[] Files { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }
        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
