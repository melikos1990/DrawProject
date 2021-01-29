using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Report;

namespace SMARTII.Domain.Case
{
    public interface ICaseAssignmentCallCenterIntegrationData
    {
        List<ExcelCaseAssignmentList> DataToDomainModel(List<SP_GetCaseAssignmentList> data, CaseAssignmentCallCenterCondition condition);
    }
}
