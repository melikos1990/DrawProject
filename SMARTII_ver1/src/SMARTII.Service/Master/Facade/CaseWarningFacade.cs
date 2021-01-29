using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class CaseWarningFacade : ICaseWarningFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        public CaseWarningFacade(IMasterAggregate MasterAggregate,
                                 IOrganizationAggregate OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Create(CaseWarning data)
        {
            #region 驗證名稱/既有單位

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                 .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseWarning_lang.CASE_WARNING_BU_FAIL);

            var isExistName = _MasterAggregate.CaseWarning_T1_
                                                 .HasAny(x => x.NODE_ID == data.NodeID &&
                                                              x.NAME == data.Name);
            if (isExistName)
                throw new Exception(CaseWarning_lang.CASE_WARNING_DUPLICATE_NAME);

            #endregion

            //取得最末排序
            var con = new MSSQLCondition<CASE_WARNING>();
            con.And(x => x.NODE_ID == data.NodeID);
            con.OrderBy(x => x.ORDER, OrderType.Desc);
            var orderIDs = _MasterAggregate.CaseWarning_T1_T2_.GetListOfSpecific(con, x => x.ORDER);

            data.Order = (orderIDs != null && orderIDs.Count() > 0) ? orderIDs.Max() + 1 : 1;
            data.CreateDateTime = DateTime.Now;
            data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            var result = _MasterAggregate.CaseWarning_T1_T2_.Add(data);
            await result.Async();
        }

        /// <summary>
        /// 更新明細
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Update(CaseWarning data)
        {

            #region 驗證名稱/既有單位

            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
                throw new Exception(CaseWarning_lang.CASE_WARNING_BU_FAIL);

            string name = data.Name.Trim();
            var isExistName = _MasterAggregate.CaseWarning_T1_T2_
                                       .HasAny(x => x.ID != data.ID &&
                                                 x.NODE_ID == data.NodeID &&
                                                 x.NAME == name);

            if (isExistName)
                throw new Exception(CaseWarning_lang.CASE_WARNING_DUPLICATE_NAME);

            #endregion

            var con = new MSSQLCondition<CASE_WARNING>(x => x.ID == data.ID);

            con.ActionModify(x =>
            {
                x.NAME = data.Name;
                x.WORK_HOUR = data.WorkHour;
                x.IS_ENABLED = data.IsEnabled;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _MasterAggregate.CaseWarning_T1_T2_.Update(con);
            await result.Async();

        }
    }
}
