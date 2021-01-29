using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Service.Organization.Facade
{
    public class UserFacade : IUserFacade
    {
        private readonly AuthenticationHelper _AuthHelper;

        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IIndex<OrganizationType, IOrganizationNodeProcessProvider> _NodeProviders;
        private readonly IExecutiveOrganizationNodeProvider<CALLCENTER_NODE> _CallCenterNodeProvider;
        private readonly IExecutiveOrganizationNodeProvider<VENDOR_NODE> _VendorNodeProvider;

        public UserFacade(IOrganizationAggregate OrganizationAggregate,
                          IMasterAggregate MasterAggregate,
                          IExecutiveOrganizationNodeProvider<VENDOR_NODE> VendorNodeProvider,
                          IExecutiveOrganizationNodeProvider<CALLCENTER_NODE> CallCenterNodeProvider,
                          IIndex<OrganizationType, IOrganizationNodeProcessProvider> NodeProviders)
        {
            _AuthHelper = new AuthenticationHelper();

            _NodeProviders = NodeProviders;
            _VendorNodeProvider = VendorNodeProvider;
            _CallCenterNodeProvider = CallCenterNodeProvider;
            _OrganizationAggregate = OrganizationAggregate;
            _MasterAggregate = MasterAggregate;
        }

        /// <summary>
        /// 取得完整的使用者資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<User> GetUserAuthFromAccountAsync(string account)
        {
            var con = new MSSQLCondition<USER>(x => x.ACCOUNT == account);

            con.IncludeBy(x => x.ROLE);
            con.IncludeBy(x => x.NODE_JOB.Select(g => g.JOB));
            con.IncludeBy(x => x.USER_PARAMETER);

            var user = _OrganizationAggregate.User_T1_T2_.GetFirstOrDefault(con);

            user.Roles?.ForEach(role =>
            {
                var merged = _AuthHelper.GetCompleteMergedPageAuth(role.Feature, user.Feature);

                role.Feature = merged;
            });

            user.JobPositions?.ForEach(async jobPosition =>
            {
                jobPosition.Node = await _NodeProviders[jobPosition.OrganizationType].Get(jobPosition.NodeID);
            });

            user.JobPositions?.GroupBy(x => x.OrganizationType).ForEach(x =>
            {
                user.DownProviderBUDist.Add(x.Key, GetDownProivderBUPair(x.ToList(), x.Key));
            });

            return await user.Async();
        }

        /// <summary>
        /// 取得完整的使用者資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<User> GetUserGroupFromIDAsync(string userID)
        {
            var con = new MSSQLCondition<USER>(x => x.USER_ID == userID);

            //con.IncludeBy(x => x.NODE_JOB.Select(g => g.USER));
            con.IncludeBy(x => x.NODE_JOB.Select(g => g.JOB));

            var user = _OrganizationAggregate.User_T1_T2_.GetFirstOrDefault(con);

            user.JobPositions.ForEach(async jobPosition =>
            {
                jobPosition.Node = await _NodeProviders[jobPosition.OrganizationType].Get(jobPosition.NodeID);
            });

            return await user.Async();
        }

        /// <summary>
        /// 往下找尋服務的BU
        /// </summary>
        /// <param name="jobPositions"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int[] GetDownProivderBUPair(List<JobPosition> jobPositions, OrganizationType type)
        {
            switch (type)
            {
                case OrganizationType.HeaderQuarter:
                    return jobPositions.Cast<HeaderQuarterJobPosition>()
                                       .Select(x => x.BUID)
                                       .Cast<int>()
                                       .ToArray();

                case OrganizationType.CallCenter:
                    return _CallCenterNodeProvider
                        .GetOwnDownwardProviderNodes(jobPositions, type)?
                        .Select(x => x.NodeID)
                        .ToArray();

                case OrganizationType.Vendor:
                    return _VendorNodeProvider
                        .GetOwnDownwardProviderNodes(jobPositions, type)?
                        .Select(x => x.NodeID)
                        .ToArray();

                default:
                    return new int[] { };
            }
        }

        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void DeleteImage(string id, string key)
        {

            var path = FilePathFormatted.GetUserImagePhysicalPath(key);

            FileUtility.DeleteFile(path);

            var con = new MSSQLCondition<USER_PARAMETER>(x => x.USER_ID == id);

            con.ActionModify(x =>
            {
                if (string.IsNullOrEmpty(x.IMAGE_PATH) == false)
                {
                    x.IMAGE_PATH = null;
                }
            });

            var result = _MasterAggregate.UserParameter_T1_T2_.Update(con);
        }
    }
}
