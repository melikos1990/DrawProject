using Autofac.Features.Indexed;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Web;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Areas.Common.Controllers
{
    [Authentication]
    public partial class OrganizationController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly IIndex<OrganizationType, IOrganizationNodeProcessProvider> _NodeProviders;
        private readonly IStoreFacade _StoreFacade;

        public OrganizationController(
            ISystemAggregate SystemAggregate,
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate,
            IOrganizationAggregate OrganizationAggregate,
            HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
            IIndex<OrganizationType, IOrganizationNodeProcessProvider> NodeProviders,
            IStoreFacade StoreFacade)
        {
            _NodeProviders = NodeProviders;
            _SystemAggregate = SystemAggregate;
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
            _StoreFacade = StoreFacade;
        }
    }
}
