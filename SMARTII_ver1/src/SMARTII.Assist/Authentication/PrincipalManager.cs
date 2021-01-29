using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;

namespace SMARTII.Assist.Authentication
{
    public class PrincipalManager : IPrincipalManager
    {
        public readonly IOrganizationAggregate _OrganizationAggregate;

        public PrincipalManager(IOrganizationAggregate OrganizationAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
        }

        public IPrincipal Generator(HttpActionContext context, User user)
        {
            if (context.Request.Headers.TryGetValues(GlobalizationCache.JobPositionKey, out IEnumerable<string> val))
            {
                user.JobPositions = GetRealTimePositions(val.First());
            }

            var identity = new UserIdentity(user, System.Threading.Thread.CurrentPrincipal.Identity);

            var principal = new GenericPrincipal(identity, null);

            return principal;
        }

        public List<JobPosition> GetRealTimePositions(string header)
        {
            var cache = JsonConvert.DeserializeObject<List<JobPosition>>(header);

            var ids = cache.Select(x => x.ID);

            // 防止 node 被拖曳造成資料不一致
            var list = _OrganizationAggregate.JobPosition_T1_T2_
                                             .GetList(x => ids.Contains(x.ID))
                                             .ToList();

            return list;
        }
    }
}