using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.KMClassification
{
    public class KMListViewModel
    {
        public KMListViewModel()
        { }
        public KMListViewModel(Domain.Master.KMData data)
        {
            this.ClassificationName = data.KMClassification.Name;
            this.Content = data.Content;
            this.ID = data.ID;
            this.Title = data.Title;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName.DisplayWhenNull();
        }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string ClassificationName { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 代號
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
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
