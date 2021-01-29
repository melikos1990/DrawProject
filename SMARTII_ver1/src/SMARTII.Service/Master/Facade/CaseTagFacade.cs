using System;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class CaseTagFacade : ICaseTagFacade
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public CaseTagFacade(ICommonAggregate CommonAggregate,
                                  ISystemAggregate SystemAggregate,
                                  IMasterAggregate MasterAggregate,
                                  IOrganizationAggregate OrganizationAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _SystemAggregate = SystemAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        public Task<CaseTag> Create(CaseTag data)
        {
            #region 驗證名稱/既有單位

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseTag_lang.CASE_TAG_BU_FAIL);

            var item = _MasterAggregate.CaseTag_T1_T2_
                                       .Get(x => x.NODE_ID == data.NodeID &&
                                                 x.NAME == data.Name);

            if (item != null)
                throw new Exception(CaseTag_lang.CASE_TAG_NAME_REPEAT);
            #endregion

            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            var result = _MasterAggregate.CaseTag_T1_T2_.Add(data);

            return result.Async();
        }

        public Task<CaseTag> Update(CaseTag data)
        {

            #region 驗證名稱/既有單位

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseTag_lang.CASE_TAG_BU_FAIL);

            string Name = data.Name.Trim();

            // 修改後的 , 其他的資料(除了本身)是否有相同
            var item = _MasterAggregate.CaseTag_T1_T2_
                                       .Get(x => x.ID != data.ID &&
                                                 x.NODE_ID == data.NodeID &&
                                                 x.NAME == Name);

            if (item != null)
                throw new Exception(CaseTag_lang.CASE_TAG_NAME_REPEAT);
            #endregion

            var con = new MSSQLCondition<CASE_TAG>(x => x.ID == data.ID);
            con.ActionModify(x =>
            {
                x.NAME = data.Name;
                x.IS_ENABLED = data.IsEnabled;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _MasterAggregate.CaseTag_T1_T2_.Update(con);

            return result.Async();
        }
    }
}
