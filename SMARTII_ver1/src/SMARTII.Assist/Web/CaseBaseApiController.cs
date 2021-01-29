using System.Linq;
using SMARTII.Domain.Organization;
using SMARTII.Service.Case.Resolver;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Provider;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Assist.Web
{
    public class CaseBaseApiController : BaseApiController
    {
        public CaseSourceResolver _CaseSourceResolver { get; set; }
        public OrganizationNodeResolver _OrganizationNodeResolver { get; set; }
        public QuestionClassificationResolver _QuestionClassificationResolver { get; set; }
        public HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider { get; set; }
        public CaseResolver _CaseResolver { get; set; }
        public StoreResolver _StoreResolver { get; set; }

        public CaseBaseApiController()
        {
        }

        /// <summary>
        /// 取得執行單位
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <returns></returns>
        protected JobPosition GetProcessNodeJob(int nodeJobID)
        {
            var jobPosition = UserIdentity.Instance
                                           .JobPositions
                                           .FirstOrDefault(x => x.ID == nodeJobID);

            var withNode = _OrganizationNodeResolver.Resolve(jobPosition);

            return withNode;
        }
    }
}
