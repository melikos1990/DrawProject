using Autofac.Features.Indexed;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Areas.Case.Controllers
{
    [Authentication]
    public partial class CaseController : CaseBaseApiController
    {
        private readonly AuthenticationHelper _AuthHelper;

        private readonly IIndex<string, IFlow> _Flows;
        private readonly ICaseService _CaseService;
        private readonly ICaseFacade _CaseFacade;
        private readonly IReportService _ReportService;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseSourceService _CaseSourceService;
        private readonly ICaseSearchService _CaseSearchService;  
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly ICaseAssignmentService _CaseAssignmentService;
        private readonly IIndex<string, IReportProvider> _ReportProviders;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;

        public CaseController(IIndex<string, IFlow> Flows,
                              ICaseService CaseService,
                              IReportService ReportService,
                              ICaseAggregate CaseAggregate,
                              ICaseFacade CaseFacade,
                              IMasterAggregate MasterAggregate,
                              ICommonAggregate CommonAggregate,
                              ICaseSearchService CaseSearchService,
                              ICaseSourceService CaseSourceService,
                              ICaseAssignmentFacade CaseAssignmentFacade,
                              ICaseAssignmentService CaseAssignmentService,
                              IIndex<string, IReportProvider> ReportProviders,
                              IOrganizationAggregate OrganizationAggregate,
                              INotificationAggregate NotificationAggregate)
        {
            _AuthHelper = new AuthenticationHelper();
            _Flows = Flows;
            _CaseService = CaseService;
            _CaseFacade = CaseFacade;
            _CaseAggregate = CaseAggregate;
            _ReportService = ReportService;
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _ReportProviders = ReportProviders;
            _CaseSearchService = CaseSearchService;
            _CaseSourceService = CaseSourceService;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _CaseAssignmentService = CaseAssignmentService;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
        }
    }
}
