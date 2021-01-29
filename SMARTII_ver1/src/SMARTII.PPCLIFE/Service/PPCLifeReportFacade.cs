using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.COMMON_BU.Service;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLifeReportFacade : ReportFacade
    {
        private readonly ICaseAggregate _CaseAggregate;

        public PPCLifeReportFacade(ICaseAggregate CaseAggregate, IMasterAggregate IMasterAggregate, ISystemAggregate SystemAggregate, IOrganizationAggregate OrganizationAggregate) : base(CaseAggregate, IMasterAggregate, SystemAggregate, OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
        }

        public override List<Case> GetSummaryCases(List<int> totalIDs, DateTime star, DateTime end, (int? BuID, string NodeKey) Bu, IEnumerable<int?> nodekeyList)
        {
            //找出底下所有分類的案件數量
            var conCase = new MSSQLCondition<CASE>();
            conCase.And(c => c.NODE_ID == Bu.BuID);
            conCase.And(c => totalIDs.Contains(c.QUESION_CLASSIFICATION_ID));
            conCase.And(c => c.CASE_COMPLAINED_USER.Any(x => nodekeyList.Contains(x.NODE_ID)));
            conCase.And(x => x.IS_REPORT == true);
            conCase.IncludeBy(c => c.CASE_SOURCE);
            conCase.IncludeBy(c => c.CASE_COMPLAINED_USER);

            conCase.And(c => c.CREATE_DATETIME > star && c.CREATE_DATETIME <= end);

            var @case = _CaseAggregate.Case_T1_T2_.GetListOfSpecific(conCase, x => new Case()
            {
                QuestionClassificationID = x.QUESION_CLASSIFICATION_ID,
                CaseSource = new CaseSource()
                {
                    CaseSourceType = (CaseSourceType)x.CASE_SOURCE.SOURCE_TYPE,
                },
            });

            return @case;
        }

        public override List<Case> GetSummaryInformationCases(List<int> totalIDs, DateTime star, DateTime end, (int? BuID, string NodeKey) Bu, IEnumerable<int?> nodekeyList)
        {
            //找出底下所有分類的案件數量
            var conCase = new MSSQLCondition<CASE>();
            conCase.And(c => c.NODE_ID == Bu.BuID);
            conCase.And(c => totalIDs.Contains(c.QUESION_CLASSIFICATION_ID));
            conCase.And(c => c.CASE_COMPLAINED_USER.Any(x => nodekeyList.Contains(x.NODE_ID)));
            conCase.And(x => x.IS_REPORT == true);
            conCase.IncludeBy(c => c.CASE_SOURCE);
            conCase.IncludeBy(c => c.CASE_COMPLAINED_USER);

            conCase.And(c => c.CREATE_DATETIME > star && c.CREATE_DATETIME <= end);

            var @case = _CaseAggregate.Case_T1_T2_.GetListOfSpecific(conCase, x => new Case()
            {
                CreateDateTime = x.CREATE_DATETIME,
                QuestionClassificationID = x.QUESION_CLASSIFICATION_ID,
                CaseSource = new CaseSource()
                {
                    CaseSourceType = (CaseSourceType)x.CASE_SOURCE.SOURCE_TYPE,
                },
            });

            return @case;
        }
    }
}
