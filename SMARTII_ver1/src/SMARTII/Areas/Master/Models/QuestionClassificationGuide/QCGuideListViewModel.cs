using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.QuestionClassificationGuide
{
    public class QCGuideListViewModel
    {
        public QCGuideListViewModel(Domain.Master.QuestionClassificationGuide data)
        {
            this.BuName = data.NodeName;
            this.Content = data.Content;
            this.ID = data.ID;
            this.NodeID = data.NodeID;
            this.ParentNodePath = Enumerable.Zip(data.ParentPathByArray, data.ParentPathNameByArray, (x, y) => new SelectItem
            {
                id = x,
                text = y
            }).ToList();
            this.Names = data.ParentPathNameByArray;
        }
        /// <summary>
        /// 識別ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 企業別代號
        /// </summary>
        public int NodeID { get; set; }
        /// <summary>
        /// 企業別名稱
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 問題分類名稱列表
        /// </summary>
        public List<SelectItem> ParentNodePath { get; set; }
        /// <summary>
        /// 回傳頁面 問題分類
        /// </summary>
        public string[] Names { get; set; }


    }
}
