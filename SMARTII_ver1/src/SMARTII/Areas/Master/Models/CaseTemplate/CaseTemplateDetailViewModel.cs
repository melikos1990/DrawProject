using System.ComponentModel.DataAnnotations;
using SMARTII.Domain.Data;

namespace SMARTII.Areas.Master.Models.CaseTemplate
{
    public class CaseTemplateDetailViewModel
    {
        public CaseTemplateDetailViewModel()
        {
        }

        public CaseTemplateDetailViewModel(Domain.Case.CaseTemplate caseTemplate)
        {
            this.ID = caseTemplate.ID;
            this.BuID = caseTemplate.NodeID;
            this.BuName = caseTemplate.NodeName;
            this.ClassificKey = caseTemplate.ClassificKey;
            this.ClassificName = caseTemplate.ClassificName;
            this.Title = caseTemplate.Title;
            this.Content = caseTemplate.Content;
            this.EmailTitle = caseTemplate.EmailTitle;
            this.IsDefault = caseTemplate.IsDefault;
            this.IsFastFinished = caseTemplate.IsFastFinished;
            this.CreateDateTime = caseTemplate.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.CreateUserName = caseTemplate.CreateUserName;
            this.UpdateDateTime = caseTemplate.UpdateDateTime.DisplayWhenNull(text: "無");
            this.UpdateUserName = caseTemplate.UpdateUserName;
        }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 範例企業別
        /// </summary>
        [Required]
        public int BuID { get; set; }

        /// <summary>
        /// 範例企業別名稱
        /// </summary>
        public string BuName { get; set; }
        /// <summary>
        /// 範例分類
        /// </summary>
        [Required]
        public string ClassificKey { get; set; }

        /// <summary>
        /// 範例分類名稱
        /// </summary>
        public string ClassificName { get; set; }

        /// <summary>
        /// 範例主旨
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "範例內容 欄位長度需要小於50")]
        public string Title { get; set; }

        /// <summary>
        /// 範例內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 信件主旨(項目)
        /// </summary>
        public string EmailTitle { get; set; }


        /// <summary>
        /// 是否為預設
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 快速結案
        /// </summary>
        public bool IsFastFinished { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string CreateDateTime { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateDateTime { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
