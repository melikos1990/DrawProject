using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Case.Facade
{
    public class CaseSourceFacade : ICaseSourceFacade
    {
        private static int _Seed = 1;
        private static object _Lock = new object();

        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;

        public CaseSourceFacade(ICaseAggregate CaseAggregate, 
                                ICommonAggregate CommonAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
        }

        /// <summary>
        /// 編碼規則
        /// </summary>
        /// <param name="date"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GeneratorCode(string dateKey, int index)
            => $"{dateKey}{index.ToString().PadLeft(5, '0')}";
        /// <summary>
        ///  取得來源編號 , 並更新滾號檔
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetSourceCode(DateTime? date = null)
        {
            var result = string.Empty;

            date = date ?? DateTime.Now.Date;

            lock (_Lock)
            {

                _CommonAggregate.Logger.Info($"IsolationLevel => {Transaction.Current.IsolationLevel}");

                var key = date.Value.ToString("yyMMdd");

                _CaseAggregate.CaseSourceCode_T1_.Operator(x =>
                {
                    var context = (SMARTIIEntities)x;
                    
                    var query = context.CASE_SOURCE_CODE.FirstOrDefault(g => g.DATE == key);
                    
                    if (query == null)
                    {
                        context.CASE_SOURCE_CODE.Add(new CASE_SOURCE_CODE()
                        {
                            DATE = key,
                            SERIAL_CODE = _Seed
                        });

                        result = GeneratorCode(key, _Seed);

                        context.SaveChanges();
                    }
                    else
                    {
                        query.SERIAL_CODE = query.SERIAL_CODE + 1;

                        context.SaveChanges();

                        result = GeneratorCode(query.DATE, query.SERIAL_CODE);
                    }

                    
                });
            }

            return result;
        }
        /// <summary>
        /// 從案件編號 , 清除預立案旗標
        /// </summary>
        /// <param name="caseIDs"></param>
        public void CancelPreventTagsFromCaseIDs(string[] caseIDs)
        {
            if (caseIDs == null || caseIDs.Count() == 0) return;

            var awaitCancellations = _CaseAggregate.CaseSource_T1_T2_
                                                   .GetList(x => x.IS_PREVENTION &&
                                                                 x.CASE.Any(g => caseIDs.Contains(g.CASE_ID)));

            if (awaitCancellations == null || awaitCancellations.Count() == 0) return;

            var targets = awaitCancellations.Select(x => x.SourceID);

            var con = new MSSQLCondition<CASE_SOURCE>(x => targets.Contains(x.SOURCE_ID));

            con.ActionModify(x =>
            {
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                x.IS_PREVENTION = false;

            });

            _CaseAggregate.CaseSource_T1_T2_.UpdateRange(con);
        }

        #region CRUD

        /// <summary>
        /// 新增來源歷程後記錄
        /// </summary>
        /// <param name="caseSourceID"></param>
        /// <param name="caseSourceHistoryPreFix"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async void CreateHistory(CaseSource caseSource, string caseSourceHistoryPreFix, User user)
        {
            try
            {
                if (caseSource == null || user == null)
                    throw new Exception(SysCommon_lang.CASE_SOURCE_INTEGRAE_NOTICE_INIT_FAIL);

                var con = new MSSQLCondition<CASE_SOURCE>(x => x.SOURCE_ID == caseSource.SourceID);
                con.IncludeBy(x => x.CASE_SOURCE_USER);

                var afterCaseSource = _CaseAggregate.CaseSource_T1_T2_.Get(con);

                if (afterCaseSource == null)
                    throw new Exception(SysCommon_lang.CASE_SOURCE_INTEGRAE_NOTICE_GET_FAIL);

                #region 寫入來源系統歷程(CaseSourceHistory)

                var caseSourceHistory = new CaseSourceHistory()
                {
                    SourceID = afterCaseSource.SourceID,
                    Content = afterCaseSource.GetObjectContentFromDescription(caseSourceHistoryPreFix),
                    CreateDateTime = DateTime.Now,
                    CreateUserName = user.Name
                };

                _CaseAggregate.CaseSourceHistory_T1_T2_.Add(caseSourceHistory);

                #endregion
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Error(caseSource.GetObjectContentFromDescription(MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion
    }
}
