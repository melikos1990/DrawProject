using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using ClosedXML.Excel;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.COMMON_BU.Service
{
    public class ReportFactory
    {
        private readonly ReportFacade _ReportFacade;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly ICommonAggregate _CommonAggregate;

        public ReportFactory(ReportFacade ReportFacade, IOrganizationAggregate OrganizationAggregate, ICommonAggregate CommonAggregate)
        {
            _ReportFacade = ReportFacade;
            _OrganizationAggregate = OrganizationAggregate;
            _CommonAggregate = CommonAggregate;
        }


        /// <summary>
        /// 驗證門市資料
        /// </summary>
        /// <param name="list"></param>
        /// <param name="HeadQuarters"></param>
        /// <param name="JobList"></param>
        public void VerifyStoreData(ref List<ImportStoreData> list, List<HeaderQuarterNode> HeadQuarters, List<Job> JobList)
        {
            foreach (var item1 in list)
            {
                if (string.IsNullOrWhiteSpace(item1.UpperOrganizationNodeName))
                    throw new Exception("上層組織節點未填寫");
                if (string.IsNullOrWhiteSpace(item1.NodeName))
                    throw new Exception("節點未填寫");
                if (string.IsNullOrWhiteSpace(item1.StoreNo))
                    throw new Exception("門市店號未填寫");
                if (string.IsNullOrWhiteSpace(item1.StoreName))
                    throw new Exception("門市名稱未填寫");

                //檢查Excel資料是否重複
                bool checkRepeatNodeName = list.Where(x => x.UpperOrganizationNodeName == item1.UpperOrganizationNodeName && x.NodeName == item1.NodeName).Count() > 1;
                if (checkRepeatNodeName)
                    throw new Exception("節點名稱重複，" + item1.NodeName);
                bool checkRepeatStore = list.Where(x => x.UpperOrganizationNodeName == item1.UpperOrganizationNodeName && x.StoreNo == item1.StoreNo && x.StoreName == item1.StoreName).Count() > 1;
                if (checkRepeatStore)
                    throw new Exception("門市重複，門市店號" + item1.StoreNo + "，門市名稱" + item1.StoreName);


                var HeadQuarterUpOrganization = HeadQuarters.Where(x => x.Name == item1.UpperOrganizationNodeName).FirstOrDefault();
                //檢查上層節點
                var checkUpperOrganizationNodeCount = HeadQuarters.Where(x => x.Name == item1.UpperOrganizationNodeName).Count();
                if (checkUpperOrganizationNodeCount == 0)
                    throw new Exception("上層組織節點不存在，組織節點：" + item1.UpperOrganizationNodeName);
                if (checkUpperOrganizationNodeCount > 1)
                    throw new Exception("上層組織節點存在一個以上，組織節點：" + item1.UpperOrganizationNodeName);
                //檢查節點是否存在
                var nodelist = HeadQuarters.Where(x => x.LeftBoundary > HeadQuarterUpOrganization.LeftBoundary && x.RightBoundary < HeadQuarterUpOrganization.RightBoundary);
                if (nodelist != null)
                {
                    var checkNode = nodelist.Any(x => x.Name == item1.NodeName);
                    if (checkNode)
                        throw new Exception("節點存在，節點：" + item1.NodeName);
                }
                //檢查節點是否存在門市代號、門市名稱
                var storeList = nodelist.Where(x => x.Store != null);
                if (storeList.Count() != 0)
                {
                    var checkStoreExist = storeList.Any(x => x.Store.Code == item1.StoreNo && x.Store.Name == item1.StoreName);
                    if (checkStoreExist)
                        throw new Exception("門市存在，門市店號" + item1.StoreNo + "，門市名稱" + item1.StoreName);
                }
                //檢查職稱是否存在並且啟用、屬於門市階層
                List<int> jobIDs = new List<int>();
                foreach (var jobName in item1.JobTitle)
                {
                    var checkJob = JobList.Any(x => x.Name == jobName);
                    if (!checkJob)
                        throw new Exception("職稱不存在，職稱：" + jobName);
                    else
                    {
                        var id = JobList.Where(x => x.Name == jobName);
                        jobIDs.Add(id.FirstOrDefault().ID);
                    }
                }
                item1.JobID = jobIDs.ToArray();
            }
        }
        /// <summary>
        /// 門市- 共通Excle資料處理
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<ImportStoreData> ImportStoreCommon(IXLTable table)
        {
            var list = new List<ImportStoreData>();
            int headRow = 0;
            foreach (var row in table.Rows())
            {
                // 跳過標題列
                if (headRow == 0)
                {
                    headRow++;
                    continue;
                }
                ImportStoreData temp = new ImportStoreData();
                // 取值
                temp.UpperOrganizationNodeName = row.Cell("2").Value.ToString();
                temp.NodeName = row.Cell("3").Value.ToString();
                temp.StoreNo = row.Cell("4").Value.ToString();
                temp.StoreName = row.Cell("5").Value.ToString();
                if (string.IsNullOrWhiteSpace(row.Cell("6").Value.ToString()))
                    throw new Exception("職稱未填寫");
                temp.JobTitle = row.Cell("6").Value.ToString().Split('@');
                temp.IsEnable = row.Cell("7").Value.ToString() == "是" ? true : false;
                list.Add(temp);
            }
            return list;
        }
        /// <summary>
        /// 匯入門市
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NodeKey"></param>
        /// <param name="defonitions"></param>
        public void ImportStoreData(List<ImportStoreData> list, string NodeKey, OrganizationNodeDefinition defonitions)
        {
            //尋找巢狀
            var conwe = new MSSQLCondition<HEADQUARTERS_NODE>();
            conwe.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            var nodes = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.GetList(conwe);
            var nested = (HeaderQuarterNode)nodes.AsNestedNSM();

            var NodeNested = nested.Children.Where(x => ((HeaderQuarterNode)x).NodeKey == NodeKey).Cast<HeaderQuarterNode>().SingleOrDefault();


            using (var transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 10, 0), TransactionScopeAsyncFlowOption.Enabled))
            {
                //新增節點
                foreach (var item in list)
                {
                    var temp = new HeaderQuarterNode();
                    temp.CreateDateTime = DateTime.Now;
                    temp.CreateUserName = GlobalizationCache.APName;
                    temp.Name = item.NodeName;
                    temp.IsEnabled = item.IsEnable;
                    temp.NodeTypeKey = defonitions.Key;
                    temp.NodeType = defonitions.ID;
                    temp.BUID = NodeNested.BUID;

                    var store = new Store();
                    store.Name = item.StoreName;
                    store.Code = item.StoreNo;
                    store.CreateDateTime = DateTime.Now;
                    store.CreateUserName = GlobalizationCache.APName;
                    store.OrganizationType = OrganizationType.HeaderQuarter;

                    _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Operator(x =>
                    {
                        var context = (SMARTIIEntities)x;

                        // 先建立Target 後 , 取得ID
                        var entity = AutoMapper.Mapper.Map<HEADQUARTERS_NODE>(temp);
                        context.HEADQUARTERS_NODE.Add(entity);
                        context.SaveChanges();

                        item.NodeID = entity.NODE_ID;

                        store.NodeID = entity.NODE_ID;
                        //寫入門市Store
                        var entity1 = AutoMapper.Mapper.Map<STORE>(store);
                        context.STORE.Add(entity1);
                        context.SaveChanges();

                        temp = AutoMapper.Mapper.Map<HeaderQuarterNode>(entity);
                    });


                    _ReportFacade.RecursiveNode(ref NodeNested, temp, item.UpperOrganizationNodeName);
                }


                //更新HeadQuarters Node 左右邊界
                var flatten = nested.FlattenNSM();
                _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Operator(x =>
                {
                    var context = (SMARTIIEntities)x;

                    // 組織結點更新
                    foreach (var item in flatten)
                    {
                        var hnode = context.HEADQUARTERS_NODE
                                           .First(g => g.NODE_ID == item.NodeID);

                        hnode.BU_ID = item.BUID;
                        hnode.LEFT_BOUNDARY = item.LeftBoundary;
                        hnode.RIGHT_BOUNDARY = item.RightBoundary;
                        hnode.DEPTH_LEVEL = item.Level;
                        hnode.PARENT_ID = item.ParentLocator;
                        hnode.PARENT_PATH = item.ParentPath;
                        hnode.UPDATE_DATETIME = DateTime.Now;
                        hnode.UPDATE_USERNAME = GlobalizationCache.APName;
                        hnode.IS_ENABLED = item.IsEnabled;

                        //更新Node Job
                        var nodeJob = context.NODE_JOB
                                             .Where(c => c.NODE_ID == hnode.NODE_ID &&
                                                         c.ORGANIZATION_TYPE == hnode.ORGANIZATION_TYPE);

                        foreach (var jobEntity in nodeJob)
                        {
                            jobEntity.IDENTIFICATION_ID = item.BUID;
                            jobEntity.LEFT_BOUNDARY = item.LeftBoundary;
                            jobEntity.RIGHT_BOUNDARY = item.RightBoundary;
                        }
                    }


                    context.SaveChanges();
                });
                //寫入新節點 Node Job
                foreach (var item in list)
                {
                    var count = flatten.Where(x => x.Name == item.NodeName && x.BUID == NodeNested.NodeID).Count();
                    _CommonAggregate.Logger.Info($"搜巡 門市名稱 {item.NodeName}, BUID {NodeNested.NodeID}, 比數 {count}");
                    var node = flatten.Where(x => x.Name == item.NodeName && x.BUID == NodeNested.NodeID).SingleOrDefault();
                    _OrganizationAggregate.JobPosition_T1_T2_.Operator(x =>
                    {
                        var context = (SMARTIIEntities)x;

                        var entities = context.JOB.Where(c => item.JobID.Contains(c.ID));

                        foreach (var entity in entities)
                        {
                            entity.NODE_JOB.Add(new NODE_JOB()
                            {
                                IDENTIFICATION_ID = node.BUID,
                                LEFT_BOUNDARY = node.LeftBoundary,
                                RIGHT_BOUNDARY = node.RightBoundary,
                                NODE_ID = node.NodeID,
                                JOB_ID = entity.ID,
                                ORGANIZATION_TYPE = (byte)node.OrganizationType
                            });
                        }
                        context.SaveChanges();
                    });
                }
                transactionscope.Complete();
            }
        }
        /// <summary>
        /// 驗證門市人員資料
        /// </summary>
        /// <param name="defonitionID"></param>
        /// <param name="BUID"></param>
        /// <param name="list"></param>
        /// <param name="storeList"></param>
        public void VerifyStorePersonnelData(int defonitionID, int? BUID, ref List<ImportStorePersonnelData> list, List<Store> storeList)
        {
            //人員清單
            var conUser = new MSSQLCondition<USER>();
            var userList = _OrganizationAggregate.User_T1_T2_.GetList(conUser);
            //門市擁有職稱清單
            var conNode_Job = new MSSQLCondition<NODE_JOB>();

            conNode_Job.And(x => x.JOB.DEFINITION_ID == defonitionID && x.IDENTIFICATION_ID == BUID);
            conNode_Job.IncludeBy(x => x.JOB);
            var nodeJobList = _OrganizationAggregate.JobPosition_T1_T2_.GetList(conNode_Job);
            if (!nodeJobList.Any())
            {
                throw new Exception("找不到職稱清單，門市匯入無新增Node_Job");
            }
            //檢查必填欄位 不可有重複資料
            foreach (var item in list)
            {
                //檢查Excel資料是否重複
                //bool checkRepeatStore = list.Where(x => x.StoreNo == item.StoreNo && x.StoreName == item.StoreName).Count() > 1;
                //if (checkRepeatStore)
                //    throw new Exception("門市重複，門市店號" + item.StoreNo + "，門市名稱" + item.StoreName);

                //檢查節點是否存在門市代號、門市名稱
                var storeCount = storeList.Where(x => x.Name == item.StoreName && x.Code == item.StoreNo);
                if (storeCount.Count() == 0)
                {
                    throw new Exception("門市不存在，門市店號" + item.StoreNo + "，門市名稱" + item.StoreName);
                }
                else if (storeCount.Count() > 1)
                {
                    throw new Exception("門市存在一個以上，門市店號" + item.StoreNo + "，門市名稱" + item.StoreName);
                }
                if (storeCount.Count() == 1)
                {
                    item.NodeID = storeCount.FirstOrDefault().NodeID;
                }
                //檢查人員
                var user = new List<User>();
                if (string.IsNullOrWhiteSpace(item.UserName))
                {
                    throw new Exception("姓名必填，未填寫");
                }
                else
                {
                    user = userList.Where(x => x.Name == item.UserName).ToList();
                }
                if (!string.IsNullOrWhiteSpace(item.Telephone))
                {
                    user = user.Where(x => x.Telephone == item.Telephone).ToList();
                }
                if (!string.IsNullOrWhiteSpace(item.EXT))
                {
                    user = user.Where(x => x.Ext == item.EXT).ToList();
                }
                if (!string.IsNullOrWhiteSpace(item.Mobile))
                {
                    user = user.Where(x => x.Mobile == item.Mobile).ToList();
                }
                if (!string.IsNullOrWhiteSpace(item.Email))
                {
                    user = user.Where(x => x.Email == item.Email).ToList();
                }
                if (user.Count() == 0)
                {
                    throw new Exception("使用者找不到，姓名:" + item.UserName + "，電話:" + item.Telephone + "，分機:" + item.EXT + "，手機:" + item.Mobile + "，Email:" + item.Email);
                }
                else if (user.Count() > 1)
                {
                    throw new Exception("使用者存在一個以上，姓名:" + item.UserName + "，電話:" + item.Telephone + "，分機:" + item.EXT + "，手機:" + item.Mobile + "，Email:" + item.Email);
                }
                else if (user.Count() == 1)
                {
                    item.UserId = user.FirstOrDefault().UserID;
                }


                //檢查職稱是否存在並且啟用、屬於門市階層
                List<int> nodeJobIDs = new List<int>();
                foreach (var jobName in item.JobTitle)
                {
                    var checkJob = nodeJobList.Any(x => x.Job.Name == jobName && x.NodeID == item.NodeID && x.Job.IsEnabled == true);
                    if (!checkJob)
                        throw new Exception("職稱不存在，職稱：" + jobName);
                    else
                    {
                        var id = nodeJobList.Where(x => x.Job.Name == jobName && x.NodeID == item.NodeID && x.Job.IsEnabled == true).FirstOrDefault().ID;
                        nodeJobIDs.Add(id);
                    }
                }
                item.NodeJobID = nodeJobIDs.ToArray();
            }
            foreach (var item in list)
            {
                bool checkRepeatStore = list.Where(x => x.StoreNo == item.StoreNo && x.StoreName == item.StoreName && x.UserId == item.UserId).Count() > 1;
                if (checkRepeatStore)
                    throw new Exception("門市人員重複，門市店號:" + item.StoreNo + "，門市名稱:" + item.StoreName + "，門市人員:" + item.UserName);
            }

        }

        /// <summary>
        /// 門市人員- 共通Excle資料處理
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<ImportStorePersonnelData> ImportStorePersonnelsCommon(IXLTable table)
        {
            var list = new List<ImportStorePersonnelData>();
            int headRow = 0;
            foreach (var row in table.Rows())
            {
                // 跳過標題列
                if (headRow == 0)
                {
                    headRow++;
                    continue;
                }
                var temp = new ImportStorePersonnelData();
                // 取值
                temp.StoreNo = row.Cell("2").Value.ToString().Trim();
                temp.StoreName = row.Cell("3").Value.ToString().Trim();
                temp.JobTitle = row.Cell("4").Value.ToString().Trim().Split('@');
                temp.UserName = row.Cell("5").Value.ToString().Trim();
                temp.Telephone = row.Cell("6").Value.ToString().Trim();
                temp.EXT = row.Cell("7").Value.ToString().Trim();
                temp.Mobile = row.Cell("8").Value.ToString().Trim();
                temp.Email = row.Cell("9").Value.ToString().Trim();
                list.Add(temp);
            }
            return list;
        }
        /// <summary>
        /// 匯入門市人員
        /// </summary>
        /// <param name="list"></param>
        /// <param name="storeList"></param>
        public void ImportStorePersonnelData(List<ImportStorePersonnelData> list, List<Store> storeList)
        {
            //寫入Node Job User
            using (var transactionscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                foreach (var item in list)
                {
                    var nodeID = storeList.Where(x => x.Name == item.StoreName && x.Code == item.StoreNo).FirstOrDefault().NodeID;

                    _OrganizationAggregate.JobPosition_T1_T2_.Operator(x =>
                    {
                        var context = (SMARTIIEntities)x;

                        var users = context.USER.First(g => g.USER_ID == item.UserId);

                        var jobPosition = context.NODE_JOB
                                          .Where(g => item.NodeJobID.Contains(g.ID))
                                          .ToList();


                        jobPosition.ForEach(c =>
                        {
                            users.NODE_JOB.Add(c);
                        });

                        context.SaveChanges();
                    });
                }
                transactionscope.Complete();
            }
        }

        /// <summary>
        /// 門市資訊 - 共通Excle資料處理
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Store> ImportStoreInfoCommon(IXLTable table)
        {

            var list = new List<Store>();
            int headRow = 0;

            //取得門市資訊
            var Stores = _OrganizationAggregate.Store_T1_T2_.GetListOfSpecific(new MSSQLCondition<STORE>(), x => new Store()
            {
                NodeID = x.NODE_ID,
                Code = x.CODE,
                Name = x.NAME
            }).ToList();

            // 開始抓出資料，並組合DomainModel
            foreach (var row in table.Rows())
            {
                // 跳過標題列
                if (headRow == 0)
                {
                    headRow++;
                    continue;
                }
                Store temp = new Store();

                if (row.Cell("2").Value == null && row.Cell("3").Value == null)
                    continue;


                // 取值
                temp.Code = row.Cell("2").Value.ToString();
                temp.Name = row.Cell("3").Value.ToString();
                temp.NodeID = Stores.FirstOrDefault(x => x.Code == temp.Code && x.Name == temp.Name).NodeID;
                temp.Telephone = row.Cell("4").Value.ToString();
                temp.Address = row.Cell("5").Value.ToString();
                temp.Email = row.Cell("6").Value.ToString();

                if (row.Cell("7").Value != null && row.Cell("7").Value.ToString() != "")
                    temp.StoreOpenDateTime = DateTime.Parse(row.Cell("7").Value.ToString());

                if (row.Cell("8").Value != null && row.Cell("8").Value.ToString() != "")
                    temp.StoreCloseDateTime = DateTime.Parse(row.Cell("8").Value.ToString());

                if (row.Cell("9").Value != null && row.Cell("9").Value.ToString() != "")
                    temp.StoreType = Int32.Parse(row.Cell("9").Value.ToString());

                temp.Memo = row.Cell("10").Value.ToString();

                if (row.Cell("11").Value != null && row.Cell("11").Value.ToString() != "")
                    temp.ServiceTime = row.Cell("11").Value.ToString();

                if (row.Cell("12").Value != null && row.Cell("12").Value.ToString() != "")
                    temp.OwnerNodeJobID = _ReportFacade.GetOwner(Stores.FirstOrDefault(x => x.Code == temp.Code && x.Name == temp.Name).NodeID, row.Cell("12").Value.ToString());

                if (row.Cell("13").Value != null && row.Cell("13").Value.ToString() != "")
                    temp.SupervisorNodeJobID = _ReportFacade.GetSupervision(Stores.FirstOrDefault(x => x.Code == temp.Code && x.Name == temp.Name).NodeID, row.Cell("13").Value.ToString());

                list.Add(temp);
            }

            return list;
        }

        public Boolean UpdateStoreInfoData(List<Store> stores)
        {
            bool result = false;

            using (var transactionscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var con = new MSSQLCondition<STORE>();

                foreach (var store in stores)
                {
                    con.And(z => z.NODE_ID == store.NodeID);
                    con.ActionModify(z =>
                    {
                        z.CODE = store.Code;
                        z.TELEPHONE = store.Telephone;
                        z.ADDRESS = store.Address;
                        z.EMAIL = store.Email;
                        z.STORE_OPEN_DATETIME = store.StoreOpenDateTime;
                        z.STORE_CLOSE_DATETIME = store.StoreCloseDateTime;
                        z.STORE_TYPE = store.StoreType;
                        z.MEMO = store.Memo;
                        z.SERVICE_TIME = store.ServiceTime;
                        z.OWNER_NODE_JOB_ID = store.OwnerNodeJobID;
                        z.SUPERVISOR_NODE_JOB_ID = store.SupervisorNodeJobID;
                        z.UPDATE_DATETIME = DateTime.Now;
                        z.UPDATE_USERNAME = GlobalizationCache.APName;

                    });

                    _OrganizationAggregate.Store_T1_T2_.Update(con);
                    con.ClearFilters();
                }

                transactionscope.Complete();

                result = true;
            }

            return result;
        }

    }
}
