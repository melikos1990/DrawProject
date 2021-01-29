using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Service.Report.Provider
{
    public class ReportProviderBase
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _IMasterAggregate;

        public ReportProviderBase(ICaseAggregate CaseAggregate,
                                  IMasterAggregate IMasterAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _IMasterAggregate = IMasterAggregate;
        }

        /// <summary>
        /// 取得案件
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="invoiceID"></param>
        /// <param name="conditionAction"></param>
        /// <returns></returns>
        public Domain.Case.Case GetCaseWithComplaint(string caseID, string invoiceID, Action<MSSQLCondition<CASE>> conditionAction)
        {
            // 取得Case資料
            var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseID);

            conditionAction?.Invoke(con);

            return _CaseAggregate.Case_T1_T2_.Get(con);
        }


        public Domain.Master.QuestionClassification GetQuestionClassification(int ID) =>
            _IMasterAggregate.QuestionClassification_T1_T2_.Get(new MSSQLCondition<QUESTION_CLASSIFICATION>(x => x.ID == ID));


        public Domain.Master.QuestionClassification GetQuestionClassificationByCode(Action<MSSQLCondition<QUESTION_CLASSIFICATION>> conditionAction = null)
        {
            var con = new MSSQLCondition<QUESTION_CLASSIFICATION>();

            if (conditionAction != null)
                conditionAction?.Invoke(con);

            return _IMasterAggregate.QuestionClassification_T1_T2_.Get(con);
        }
        /// <summary>
        /// 取得改善方式ID
        /// </summary>
        /// <returns></returns>
        public int GetClassification()
        {
            var conReasonClass = new MSSQLCondition<CASE_FINISH_REASON_CLASSIFICATION>();

            conReasonClass.And(x => x.KEY == ReasonClassValue.RECOGNIZE);
            var reasonClass = _IMasterAggregate.CaseFinishReasonClassification_T1_T2_.Get(conReasonClass);
            return reasonClass.ID;
        }
    }
}
