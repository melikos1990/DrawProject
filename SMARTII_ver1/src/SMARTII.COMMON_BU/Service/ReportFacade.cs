using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Service.Cache;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.COMMON_BU.Service
{
    public class ReportFacade
    {
        private readonly IMasterAggregate _IMasterAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public ReportFacade(ICaseAggregate CaseAggregate, IMasterAggregate IMasterAggregate, ISystemAggregate SystemAggregate, IOrganizationAggregate OrganizationAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _IMasterAggregate = IMasterAggregate;
            _SystemAggregate = SystemAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }
        /// <summary>
        /// 本月/本日彙總表
        /// </summary>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <param name="Bu"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<SummaryInfoData> GetSummaryDataAsync(DateTime star, DateTime end, (int? BuID, string NodeKey) Bu)
        {

            SummaryInfoData summaryInfoData = new SummaryInfoData();

            //取得問題分類物件
            var includeStr = "Select".NestedLambdaString("QUESTION_CLASSIFICATION1", "c => c.QUESTION_CLASSIFICATION1", 5);
            var expression = await typeof(QUESTION_CLASSIFICATION).IncludeExpression<Func<QUESTION_CLASSIFICATION, object>>(includeStr);

            var con = new MSSQLCondition<QUESTION_CLASSIFICATION>();
            con.IncludeBy(expression);
            con.And(c => c.LEVEL == 1);
            con.And(c => c.NODE_ID == Bu.BuID);
            //所有大分類項目
            List<QuestionClassification> qcfList = _IMasterAggregate.QuestionClassification_T1_T2_.GetList(con).ToList();

            //取得Nodekey範圍
            var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHS.And(x => x.NODE_KEY == Bu.NodeKey);
            var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);
            var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
            conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
            var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList);
            var nodekeyList = headerList.Select(x => x.NodeID).Cast<int?>();

            summaryInfoData.QuestionClassifications.AddRange(qcfList);

            foreach (var item in qcfList)
            {
                List<int> totalIDs = new List<int>();

                //遞迴攤平項目子節點
                var childs = item.Flatten().Where(x => x.Level != 1);
                totalIDs.AddRange(childs.Select(c => c.ID));
        
                summaryInfoData.Cases.Add((item.ID, GetSummaryCases(totalIDs, star, end, Bu, nodekeyList)));

            }

            //加入來源項目
            summaryInfoData.SelectItems = DataStorage.CaseSourceDict.FirstOrDefault(c => c.Key == Bu.NodeKey).Value;

            return summaryInfoData;
        }

        public virtual List<Domain.Case.Case> GetSummaryCases(List<int> totalIDs, DateTime star, DateTime end, (int? BuID, string NodeKey) Bu, IEnumerable<int?> nodekeyList)
        {
            //找出底下所有分類的案件數量
            var conCase = new MSSQLCondition<CASE>();
            conCase.And(c => c.NODE_ID == Bu.BuID);
            conCase.And(c => totalIDs.Contains(c.QUESION_CLASSIFICATION_ID));
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

        /// <summary>
        /// 彙總資訊
        /// </summary>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <param name="Bu"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<SummaryInfoData> GetSummaryInformationDataAsync(DateTime star, DateTime end, (int? BuID, string NodeKey) Bu)
        {
            {
                SummaryInfoData summaryInfoData = new SummaryInfoData();

                //取得問題分類物件
                var includeStr = "Select".NestedLambdaString("QUESTION_CLASSIFICATION1", "c => c.QUESTION_CLASSIFICATION1", 5);
                var expression = await typeof(QUESTION_CLASSIFICATION).IncludeExpression<Func<QUESTION_CLASSIFICATION, object>>(includeStr);

                var con = new MSSQLCondition<QUESTION_CLASSIFICATION>();
                con.IncludeBy(expression);
                con.And(c => c.LEVEL == 1);
                con.And(c => c.NODE_ID == Bu.BuID);
                //所有大分類項目
                List<QuestionClassification> qcfList = _IMasterAggregate.QuestionClassification_T1_T2_.GetList(con).ToList();

                //取得Nodekey範圍
                var conHS = new MSSQLCondition<HEADQUARTERS_NODE>();
                conHS.And(x => x.NODE_KEY == Bu.NodeKey);
                var header = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(conHS);
                var conHSList = new MSSQLCondition<HEADQUARTERS_NODE>();
                conHSList.And(x => x.LEFT_BOUNDARY >= header.LeftBoundary && x.RIGHT_BOUNDARY <= header.RightBoundary);
                var headerList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conHSList);
                var nodekeyList = headerList.Select(x => x.NodeID).Cast<int?>();

                summaryInfoData.QuestionClassifications.AddRange(qcfList);

                foreach (var item in qcfList)
                {
                    List<int> totalIDs = new List<int>();

                    //遞迴攤平項目子節點
                    var childs = item.Flatten().Where(x => x.Level != 1);
                    totalIDs.AddRange(childs.Select(c => c.ID));

                    summaryInfoData.Cases.Add((item.ID, GetSummaryInformationCases(totalIDs, star,end,Bu, nodekeyList)));
                }

                //加入來源項目
                summaryInfoData.SelectItems = DataStorage.CaseSourceDict.FirstOrDefault(c => c.Key == Bu.NodeKey).Value;

                return summaryInfoData;
            }
        }

        public virtual List<Domain.Case.Case> GetSummaryInformationCases(List<int> totalIDs, DateTime star, DateTime end, (int? BuID, string NodeKey) Bu,IEnumerable<int?> nodekeyList)
        {
            //找出底下所有分類的案件數量
            var conCase = new MSSQLCondition<CASE>();
            conCase.And(c => c.NODE_ID == Bu.BuID);
            conCase.And(c => totalIDs.Contains(c.QUESION_CLASSIFICATION_ID));
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

        public int GetOwner(int NodeID, string OwnerName)
        {
            HeaderQuarterNode hqnode = GetNodeIDbyHead(NodeID);

            if (hqnode == null) ThrowError(NodeID);

            var con = new MSSQLCondition<NODE_JOB>();
            con.IncludeBy(x => x.JOB);
            con.And(x => x.NODE_ID == hqnode.NodeID);

            var NodeJobs = _OrganizationAggregate.JobPosition_T1_T2_.GetListOfSpecific(con, x => new JobPosition()
            {
                ID = x.ID,
                Job = new Job()
                {
                    Name = x.JOB.NAME
                }
            }).ToList();

            if (NodeJobs.Any() == false || NodeJobs.FirstOrDefault(x => x.Job.Name == OwnerName) == null)
            {
                ThrowError(hqnode.NodeID);
            }

            return NodeJobs.FirstOrDefault(x => x.Job.Name == OwnerName).ID;
        }


        /// <summary>
        /// 取得該部門 職稱OFC
        /// </summary>
        /// <param name="NodeID"></param>
        /// <param name="Supervision"></param>
        /// <returns></returns>
        public int GetSupervision(int NodeID, string Supervision)
        {
            var firstSup = Supervision.Split('@');

            if (firstSup.Any() == false) throw new Exception("欄位 OFC職稱(組織名稱@職稱)解析錯誤");

            HeaderQuarterNode hqnode = GetNodeIDbyHead(NodeID);

            if (hqnode == null) ThrowError(NodeID);
            var conNode = new MSSQLCondition<HEADQUARTERS_NODE>();
            string NodeName = firstSup[0];
            conNode.And(x => x.BU_ID == hqnode.BUID && x.NAME == NodeName);
            var Node = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetFirstOrDefault(conNode);

            if (Node == null)
            {
                throw new Exception($"查無此組織名稱: {firstSup[0]}");
            }
            var ID = Node.NodeID;
            var con = new MSSQLCondition<NODE_JOB>();
            con.IncludeBy(x => x.JOB);
            con.And(x => x.NODE_ID == ID);

            var NodeJobs = _OrganizationAggregate.JobPosition_T1_T2_.GetListOfSpecific(con, x => new JobPosition()
            {
                ID = x.ID,
                Job = new Job()
                {
                    Name = x.JOB.NAME,
                    Key = x.JOB.KEY
                }
            }).ToList();

            if (NodeJobs.Any() == false)
            {
                new Exception($"查無此NodeID 職稱清單，NodeID: {Node.NodeID}");
            }
            if (NodeJobs.FirstOrDefault(x => x.Job.Name == firstSup[1] && x.Job.Key == JobValue.OFC) == null)
            {
                throw new Exception($"查無此職稱OFC Job職稱: {firstSup[1]}");
            }

            return NodeJobs.FirstOrDefault(x => x.Job.Name == firstSup[1] && x.Job.Key == JobValue.OFC).ID;
        }

        private static void ThrowError(int NodeID) => throw new Exception($"查無此NodeJob {NodeID}主檔");

        private HeaderQuarterNode GetNodeIDbyHead(int NodeID)
        {
            return _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == NodeID));
        }


        public void RecursiveNode(ref HeaderQuarterNode node, HeaderQuarterNode data, string upperOrganizationNodeName)
        {
            if (node.Children != null && node.Children.Any())
            {
                //尋覽底下的
                foreach (var item in node.Children)
                {
                    var child = (HeaderQuarterNode)item;
                    if (child.Name == upperOrganizationNodeName)
                    {
                        child.Children.Add(data);
                        break;
                    }
                    else
                    {
                        RecursiveNode(ref child, data, upperOrganizationNodeName);
                    }
                }
            }
        }
    }
}
