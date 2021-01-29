using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;

namespace SMARTII.ASO
{
    public class DailyReport
    {
        /// <summary>
        /// 來源
        /// </summary>
        public CaseSourceType CaseSourceType { get; set; }
        /// <summary>
        /// 問題分類
        /// </summary>
        public QuestionClassification ClassList { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 祖父階層問題大分類名稱
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 細項分類名稱
        /// </summary>
        public string ChildrenName { get; set; }
    }
}
