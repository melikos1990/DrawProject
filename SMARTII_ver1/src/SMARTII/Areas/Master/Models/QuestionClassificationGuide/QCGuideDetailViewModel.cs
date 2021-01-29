using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.QuestionClassificationGuide
{
    public class QCGuideDetailViewModel
    {
        public QCGuideDetailViewModel()
        {
        }
        public Domain.Master.QuestionClassificationGuide ToDomain()
        {
            var result = new Domain.Master.QuestionClassificationGuide();
            result.ID = this.ID;
            result.ClassificationID = this.ClassificationID;
            result.OrganizationType = OrganizationType.HeaderQuarter;
            result.NodeID = this.NodeID;
            result.Content = this.Content;
            return result;
        }
        public QCGuideDetailViewModel(Domain.Master.QuestionClassificationGuide data)
        {
            this.ID = data.ClassificationID;
            this.NodeID = data.NodeID;
            this.BuName = data.NodeName;
            this.ClassificationID = data.ClassificationID;
            this.ClassificationName = data.ClassificationName;
            this.Content = data.Content;
            this.ParentNodePath = Enumerable.Zip(data.ParentPathByArray, data.ParentPathNameByArray, (x, y) => new SelectItem
            {
                id = x,
                text = y
            }).ToList(); ;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;
        }
        /// <summary>
        ///   識別ID  
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 問題分類ID
        /// </summary>
        public int ClassificationID { get; set; }
        /// <summary>
        /// 問題分類名稱
        /// </summary>
        public string ClassificationName { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
        /// <summary>
        /// 更新人員
        /// </summary>
        public string UpdateUserName { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }
        /// <summary>
        /// 問題分類名稱列表
        /// </summary>
        public List<SelectItem> ParentNodePath { get; set; }
        
    }
}
