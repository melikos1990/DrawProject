using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ClosedXML.Excel;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Service.Report.Factory
{
    public class ImportQuestionClassificationFactory : IImportQuestionClassificationFactory
    {

        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IMasterAggregate _IMasterAggregate;

        public ImportQuestionClassificationFactory(IOrganizationAggregate OrganizationAggregate, IMasterAggregate MasterAggregate)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _IMasterAggregate = MasterAggregate;
        }

        /// <summary>
        /// 匯入問題分類
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="NodeKey"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool ImportQuestionClassification(IXLWorksheet worksheet, string NodeKey, out string ErrorMessage)
        {
            ErrorMessage = "";
            var success = true;
            try
            {
                // 定義資料起始、結束 Cell
                var firstCell = worksheet.FirstCellUsed();
                var lastCell = worksheet.LastCellUsed();

                // 使用資料起始、結束 Cell，來定義出一個資料範圍
                var data = worksheet.Range(firstCell.Address, lastCell.Address);

                // 將資料範圍轉型
                var table = data.AsTable();

                // 開始抓出資料，並組合DomainModel
                var list = ImportQuestionClassificationCommon(table);

                var con = new MSSQLCondition<HEADQUARTERS_NODE>();
                con.And(x => x.NODE_KEY == NodeKey);
                var Node = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(con);


                //檢查必填欄位 不可有重複資料
                VerifyQuestionClassificationData(list);

                //匯入問題分類
                ImportQuestionClassificationData(list, Node.NodeID, 1);
            }

            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 問題分類- 共通Excle資料處理
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<ImportQuestionClassificationData> ImportQuestionClassificationCommon(IXLTable table)
        {
            var list = new List<ImportQuestionClassificationData>();
            int headRow = 0;
            foreach (var row in table.Rows())
            {
                // 跳過標題列
                if (headRow == 0)
                {
                    headRow++;
                    continue;
                }
                var temp = new ImportQuestionClassificationData();
                temp.QuestionClassificationList = new List<string>();
                int level = 1;
                // 取值
                for (int i = 2; i < table.Columns().Count(); i++)
                {

                    string text = row.Cell(i.ToString()).Value.ToString();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        temp.QuestionClassificationList.Add(text);
                        temp.Level = level;
                        level++;
                    }
                    else
                    {
                        break;
                    }
                }
                temp.IsEnable = row.Cell(table.Columns().Count()).Value.ToString() == "啟用" ? true : false;

                list.Add(temp);
            }
            return list;
        }

        /// <summary>
        /// 驗證問題分類資料
        /// </summary>
        /// <param name="list"></param>
        /// <param name="HeadQuarters"></param>
        /// <param name="JobList"></param>
        public void VerifyQuestionClassificationData(List<ImportQuestionClassificationData> list)
        {
            var levelCount = list.Max(x => x.Level);
            Verify(list, levelCount);

            //VerifyRankToDB(list, 1);
        }

        public void Verify(List<ImportQuestionClassificationData> list, int level)
        {
            //從最底層資料做群組，找出重複的資料
            var g = list.Where(x => x.Level == level).GroupBy(
                x => new
                {
                    level = string.Join(",", x.QuestionClassificationList),
                }
                ).Select(c => c.ToList()).ToList();

            if (g.Any(x => x.Count > 1))
            {
                List<ImportQuestionClassificationData> importQuestionClassificationDatas = g.FirstOrDefault(x => x.Count > 1).Select(x => x).ToList();
                var name = string.Join("|", importQuestionClassificationDatas.FirstOrDefault().QuestionClassificationList);
                throw new Exception($"重複問題分類名稱，問題分類名稱:{name},有{importQuestionClassificationDatas.Count}筆重複");
            }
        }

        public void VerifyRankToDB(List<ImportQuestionClassificationData> list, int level)
        {
            var tempGroupName = list.GroupBy(x => x.QuestionClassificationList[level - 1]).Select(c => c.ToList()).ToList();

            foreach (var item in tempGroupName)
            {
                var tempLevelList = item.Where(x => x.Level == level).GroupBy(c => c.QuestionClassificationList[level - 1]).ToList();


                if (tempLevelList.Count() == 0)
                {
                    var name = item.Select(x => x.QuestionClassificationList[level - 1]);
                    throw new Exception($"第{level}層沒有設定，該問題分類名稱:{string.Join("\t\n", name)}");
                }

                var test = new object();

                if (level == 3)
                {

                    test = tempLevelList.GroupBy(c =>
                    {

                        return new { ParentName = c.FirstOrDefault(x => x.Level == level).QuestionClassificationList.ElementAtOrDefault(level - 2), Name = c.Key };
                    });//.Any(x => x.Count() > 1);
                }

                var groupby = tempLevelList.SelectMany(c => c.ToList()).GroupBy(x => new { ParentName = x.QuestionClassificationList.ElementAtOrDefault(level - 2), Name = x.QuestionClassificationList.ElementAtOrDefault(level - 1) });
                var cou = groupby.Where(x => x.Count() > 1);
                if (groupby.Any(x => x.Count() > 1))
                {
                    //var name = tempLevelList.Select(c => c).Where(x => x.Count() > 1).First().First().QuestionClassificationList[level - 1];
                    var name = groupby.FirstOrDefault().Key.Name;
                    throw new Exception($"第{level}層有重複問題分類名稱，問題分類名稱:{name}");
                }
                level++;
                if (item.Any(x => x.Level >= level))
                {
                    var df = item.Where(x => x.Level >= level).ToList();
                    VerifyRankToDB(df, level);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 匯入問題分類
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NodeKey"></param>
        /// <param name="defonitions"></param>
        public void ImportQuestionClassificationData(List<ImportQuestionClassificationData> list, int NodeID, int level)
        {
            using (var transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 10, 0), TransactionScopeAsyncFlowOption.Enabled))
            {
                insert(list, 1, NodeID);
                transactionscope.Complete();
            }
        }

        public void insert(List<ImportQuestionClassificationData> list, int level, int NodeID)
        {
            var tempGroupName = list.GroupBy(x => x.QuestionClassificationList[level - 1]).Select(c => c.ToList()).ToList();

            //新增
            foreach (var item in tempGroupName)
            {
                var tempLevelList = item.Where(x => x.Level == level).GroupBy(c => c.QuestionClassificationList[level - 1]).Select(c => c.ToList()).ToList();

                foreach (var rankData in tempLevelList)
                {
                    foreach (var data in rankData)
                    {
                        var temp = new QuestionClassification()
                        {
                            Name = data.QuestionClassificationList[level - 1],
                            NodeID = NodeID,
                            ParentID = data.ParentID,
                            Level = level,
                            IsEnabled = data.IsEnable,
                            OrganizationType = OrganizationType.HeaderQuarter,
                            CreateUserName = LoginValue.SYSTEM_SETTING,
                            CreateDateTime = DateTime.Now
                        };
                        int? parentID = 0;
                        _IMasterAggregate.QuestionClassification_T1_T2_.Operator(x =>
                        {
                            var context = (SMARTIIEntities)x;

                            // 先建立Target 後 , 取得ID
                            var entity = AutoMapper.Mapper.Map<QUESTION_CLASSIFICATION>(temp);
                            context.QUESTION_CLASSIFICATION.Add(entity);
                            context.SaveChanges();
                            parentID = entity.ID;
                        });

                        //將PARENTID 填入下一層中
                        var df = item.Where(g => g.QuestionClassificationList[level - 1] == data.QuestionClassificationList[level - 1]).ToList();
                        foreach (var c in df)
                        {
                            c.ParentID = parentID;
                        }
                    }
                }
                if (item.Any(x => x.Level >= level + 1))
                {
                    var df = item.Where(x => x.Level >= level + 1).ToList();
                    insert(df, level + 1, NodeID);
                }
            }



        }
    }
}
