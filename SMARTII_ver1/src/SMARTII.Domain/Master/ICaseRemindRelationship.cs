using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Master
{
    public interface ICaseRemindRelationship
    {

        string CaseID { get; set; }

        int AssignmentID { get; set; }
            
        List<CaseRemind> CaseReminds { get; set; }

    }
}
