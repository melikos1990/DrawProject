namespace SMARTII.Areas.Master.Models.CaseTemplate
{
    public class CaseTemplateListViewModel
    {
        public CaseTemplateListViewModel()
        {
        }

        public CaseTemplateListViewModel(Domain.Case.CaseTemplate caseTemplate)
        {
            this.ID = caseTemplate.ID;
            this.BuID = caseTemplate.NodeID;
            this.BuName = caseTemplate.NodeName;
            this.ClassificKey = caseTemplate.ClassificKey;
            this.ClassificName = caseTemplate.ClassificName;
            this.Title = caseTemplate.Title;
            this.Content = caseTemplate.Content;
            this.EmailTitle = caseTemplate.EmailTitle;
        }

        /// <summary>
        /// 識別規格
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 識別規格
        /// </summary>
        public string BuName { get; set; }

        /// <summary>
        /// 範例企業別
        /// </summary>
        public int BuID { get; set; }

        /// <summary>
        /// 範例分類
        /// </summary>
        public string ClassificKey { get; set; }

        /// <summary>
        /// 範例分類名稱
        /// </summary>
        public string ClassificName { get; set; }

        /// <summary>
        /// 範例主旨
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 範例內容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 信件主旨(項目)
        /// </summary>
        public string EmailTitle { get; set; }
    }
}
