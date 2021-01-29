using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.QuestionClassificationAnswer
{
    public class QuestionClassificationAnswerListViewModel
    {
        public QuestionClassificationAnswerListViewModel(Domain.Master.QuestionClassificationAnswer data)
        {

            this.ID = data.ID;
            this.BuName = data.NodeName;
            this.ParentNodePath = Enumerable.Zip(data.ParentPathByArray, data.ParentPathNameByArray, (x, y) => new SelectItem
            {
                id = x,
                text = y
            }).ToList();
            this.Names = data.ParentPathNameByArray;
            this.Content = data.Content;
            this.Title = data.Title;
        }
        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 問題分類名稱列表
        /// </summary>
        public List<SelectItem> ParentNodePath { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 主旨
        /// </summary>
        public string Title { get; set; }

        public string[] Names { get; set; }
    }
}
