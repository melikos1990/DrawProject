using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Report
{
    public interface IBatchReportProvider
    {
        byte[] GenerateCaseRemindReport(List<CaseRemind> caseReminds);
    }
}
