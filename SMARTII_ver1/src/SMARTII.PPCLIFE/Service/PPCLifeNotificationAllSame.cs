using System;
using System.Collections.Generic;
using System.Linq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Notification;
using SMARTII.PPCLIFE.Domain;

namespace SMARTII.PPCLIFE.Service
{
    public class PPCLifeNotificationAllSame : PPCLifeNotificationBase, IPPCLifeNotificationFactory
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICommonAggregate _CommonAggregate;

        public PPCLifeNotificationAllSame(ICaseAggregate CaseAggregate,
                                          ICommonAggregate CommonAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _CommonAggregate = CommonAggregate;
        }

        /// <summary>
        /// 計算達標-規則:同批號同產品
        /// </summary>
        public List<PPCLifeEffectiveSummary> Execute(List<CasePPCLife> casePPCLives)
        {
            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  準備進行 同批號同產品 規則比對。");

            var result = new List<PPCLifeEffectiveSummary>();

            var realCasePPCLives = casePPCLives.Where(x => x.IsIgnore == false &&
                                                 x.AllSameFinish == false).ToList();

            if (realCasePPCLives == null)
                throw new Exception("無案件比對");

            var caseItems = new List<CaseItem>();

            foreach (var casePPCLife in realCasePPCLives)
            {
                var pCon = new MSSQLCondition<CASE_ITEM>();
                pCon.IncludeBy(x => x.CASE);
                pCon.IncludeBy(x => x.ITEM);
                var caseItem = _CaseAggregate.CaseItem_T1_T2_.Get(x => x.CASE_ID == casePPCLife.CaseID && x.ITEM_ID == casePPCLife.ItemID);
                caseItems.Add(caseItem);
            }

            var rules = base.GeneratePPCLifeRule(caseItems).GroupBy(x => new { x.ItemID, x.BatchNo}).Select(x=> x.First()).Where(x => !string.IsNullOrEmpty(x.BatchNo)).ToList();

            foreach (var rule in rules)
            {
                var count = 0;
                var arriveCaseItems = new List<CaseItem>();

                foreach (var caseItem in caseItems)
                {
                    if (rule.ItemID == caseItem.ItemID && rule.BatchNo == (caseItem.GetParticular<PPCLIFE_CaseItem>().BatchNo ?? ""))
                    {
                        count += 1;
                        arriveCaseItems.Add(caseItem);
                    }
                }

                // 是否達到標準-同批號同產品，含3筆案件
                if (count >= 3)
                {
                    var pPCLifeEffectiveSummary = new PPCLifeEffectiveSummary()
                    {
                        PPCLifeArriveType = PPCLifeArriveType.AllSame,
                        ItemID = rule.ItemID,
                        BatchNo = rule.BatchNo,
                        CreateDateTime = DateTime.Now,
                        CreateUserName = GlobalizationCache.APName,
                        CasePPCLifes = base.GenerateCasePPCLife(arriveCaseItems)
                    };

                    result.Add(pPCLifeEffectiveSummary);
                }
            }

            _CommonAggregate.Logger.Info($"【統藥大量叫修通知】  同批號同產品 規則比對完畢。");

            return result;
        }
    }
}
