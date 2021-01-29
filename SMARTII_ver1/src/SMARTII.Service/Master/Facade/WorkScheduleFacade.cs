using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class WorkScheduleFacade : IWorkScheduleFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public WorkScheduleFacade(IMasterAggregate MasterAggregate,
                                  ICaseAggregate CaseAggregate,
                                       IOrganizationAggregate OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        

        /// <summary>
        /// 新增-特定假日
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Create(List<WorkSchedule> data)
        {
            DateTime now = DateTime.Now.Date.AddDays(1);


            ErrorProcessHelp.Invoker<WorkSchedule>(context =>
            {

                var dateGroup = data.GroupBy(x => new
                {
                    x.Date,
                    x.NodeID
                }).Where(x => x.Count() > 1);

                if (dateGroup.Any())
                    context.AddRange(dateGroup.SelectMany(x => x).ToList());


                var buIDs = data.Select(x => x.NodeID).Cast<int?>().ToList();

                var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                        .HasAny(x => buIDs.Contains(x.BU_ID));
                if (!isExitHeadquarters)
                    throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_BU_FAIL);

                //如果驗證日期先重複，先查出驗證日期重複的資料，反之如果案件時效內先有資料，則先查詢案件時效內的資料
                bool ExistEarly = true;
                bool ExistTime = true;
                bool ExistCase = true;
                foreach (var item in data)
                {
                    #region 是否早於當下時間/驗證時間/既有單位
                    if (ExistEarly)
                    {
                        if (item.Date < DateTime.Now)
                        {
                            context.Add(item);
                            ExistCase = false;
                            ExistTime = false;
                        }
                    }

                    if (ExistTime)
                    {
                        //驗證日期是否重複
                        var isExistTime = _MasterAggregate.WorkSchedule_T1_
                                                             .HasAny(x => x.NODE_ID == item.NodeID && x.DATE == item.Date);
                        if (isExistTime)
                        {
                            context.Add(item);
                            ExistCase = false;
                            ExistEarly = false;
                        }
                    }

                    if (ExistCase)
                    {
                        //檢查是否有案件在計算時效，若有則無法編輯

                        var isExistCase = _CaseAggregate.Case_T1_.HasAny(x => x.NODE_ID == item.NodeID && x.PROMISE_DATETIME > item.Date);

                        if (isExistCase)
                        {
                            context.Add(item);
                            ExistTime = false;
                            ExistEarly = false;
                        }
                    }


                    #endregion

                    item.CreateDateTime = DateTime.Now;
                    item.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
                }

                if (context.InValid())
                {
                    if (ExistEarly)
                        context.Message = WorkSchedule_lang.WORK_SCHEDULE_DATE_ILLEGAL;
                    else if (ExistTime)
                        context.Message = WorkSchedule_lang.WORK_SCHEDULE_DUPLICATE_TIME;
                    else
                        context.Message = WorkSchedule_lang.WORK_SCHEDULE_CASE_INAGING;
                }

            });

            var result = _MasterAggregate.WorkSchedule_T1_T2_.AddRange(data);

            await result.Async();

        }
        /// <summary>
        /// 更新-特定假日
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task Update(WorkSchedule data)
        {
            #region 驗證名稱/既有單位
            var isExitHeadquarters = _OrganizationAggregate.HeaderQuarterNode_T1_
                                                     .HasAny(x => x.BU_ID == data.NodeID);
            if (!isExitHeadquarters)
            {
                throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_BU_FAIL);
            }
            var isExistTime = _MasterAggregate.WorkSchedule_T1_
                                                 .HasAny(x => x.DATE == data.Date && 
                                                 x.ID != data.ID && 
                                                 x.NODE_ID == data.NodeID);
            if (isExistTime)
            {
                throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_DUPLICATE_TIME);
            }
            //檢查是否有案件在計算時效，若有則無法編輯

            var isExistCase = _CaseAggregate.Case_T1_.HasAny(x => x.NODE_ID == data.NodeID && x.PROMISE_DATETIME > data.Date);

            if (isExistCase)
            {
                throw new Exception(WorkSchedule_lang.WORK_SCHEDULE_CASE_INAGING);
            }
            #endregion

            // 組合 con 物件
            var con = new MSSQLCondition<WORK_SCHEDULE>(x => x.ID == data.ID);

            con.ActionModify(x =>
            {
                x.TITLE = data.Title;
                x.WORK_TYPE = (byte)data.WorkType;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            var result = _MasterAggregate.WorkSchedule_T1_T2_.Update(con);

            await result.Async();
        }


        public async Task<bool> ChackIncorporatedCase(int id)
        {

            var con = new MSSQLCondition<WORK_SCHEDULE>(x => x.ID == id);


            var data = _MasterAggregate.WorkSchedule_T1_T2_.GetOfSpecific(con, x => new { x.DATE, x.NODE_ID });


            return _CaseAggregate.Case_T1_T2_.HasAny(x => x.PROMISE_DATETIME >= data.DATE && x.NODE_ID == data.NODE_ID);
        }

    }
}
