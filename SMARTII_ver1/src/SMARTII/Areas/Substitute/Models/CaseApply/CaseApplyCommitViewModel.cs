using System.Collections.Generic;

namespace SMARTII.Areas.Substitute.Models.CaseApply
{
    public class CaseApplyCommitViewModel
    {
        public CaseApplyCommitViewModel()
        {
        }

        public string ApplyUserID { get; set; }

        public List<string> CaseIDs { get; set; }
    }
}