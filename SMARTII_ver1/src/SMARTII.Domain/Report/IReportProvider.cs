using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Case;

namespace SMARTII.Domain.Report
{
    public interface IReportProvider
    {
        BaseComplaintReport GeneratorPayload(string caseID, string invoiceID);

        HttpFile ComplaintedReport(BaseComplaintReport baseComplaintReport);
    }
}
