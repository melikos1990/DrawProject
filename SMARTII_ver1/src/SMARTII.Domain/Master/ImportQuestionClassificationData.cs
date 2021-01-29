using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMARTII.Domain.Master
{
    public class ImportQuestionClassificationData
    {
        public List<string> QuestionClassificationList { get; set; }

        public int? ParentID { get; set; }

        public int Level { get; set; }

        public bool IsEnable { get; set; }


    }
}
