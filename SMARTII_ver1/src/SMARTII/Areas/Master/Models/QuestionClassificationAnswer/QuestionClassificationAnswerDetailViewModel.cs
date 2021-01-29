using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Master.Models.QuestionClassificationAnswer
{
    public class QuestionClassificationAnswerDetailViewModel
    {
        public QuestionClassificationAnswerDetailViewModel()
        {

        }
        public QuestionClassificationAnswerDetailViewModel(Domain.Master.QuestionClassificationAnswer data)
        {
            this.NodeID = data.NodeID;
            this.BuName = data.NodeName;
            this.ClassificationID = data.ClassificationID;
            this.ID = data.ID;
            this.Title = data.Title;
            this.Content = data.Content;
            this.CreateDateTime = data.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = data.CreateUserName;
            this.UpdateDateTime = data.UpdateDateTime.DisplayWhenNull();
            this.UpdateUserName = data.UpdateUserName;
            this.ParentNodePath = Enumerable.Zip(data.ParentPathByArray, data.ParentPathNameByArray, (x, y) => new SelectItem
            {
                id = x,
                text = y
            }).ToList();
        }
        public Domain.Master.QuestionClassificationAnswer ToDomain()
        {
            var result = new Domain.Master.QuestionClassificationAnswer();
            result.ID = this.ID;
            result.ClassificationID = this.ClassificationID;
            result.Title = this.Title;
            result.Content = this.Content;
            result.NodeID = this.NodeID;
            return result;
        }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 問題分類代號
        /// </summary>
        public int ClassificationID { get; set; }
        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 主旨
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 組織型態定義
        /// </summary>
        public OrganizationType OrganizationType { get; set; }
        public List<SelectItem> ParentNodePath { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }
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
