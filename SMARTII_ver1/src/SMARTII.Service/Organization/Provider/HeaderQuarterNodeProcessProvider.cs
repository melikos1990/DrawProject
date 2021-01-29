using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Provider
{
    public class HeaderQuarterNodeProcessProvider : IOrganizationNodeProcessProvider
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, IOrganizationProcessStrategy> _Strategies;
        private readonly ICaseAggregate _CaseAggregate;

        public HeaderQuarterNodeProcessProvider(IOrganizationAggregate OrganizationAggregate,
                                        IIndex<string, IOrganizationProcessStrategy> Strategies,
                                        ICaseAggregate CaseAggregate)
        {
            _Strategies = Strategies;
            _OrganizationAggregate = OrganizationAggregate;
            _CaseAggregate = CaseAggregate;
        }

        public async Task Create(IOrganizationNode node)
        {
            var data = (HeaderQuarterNode)node;
            var flatten = data.FlattenNSM();

            var target = flatten.FirstOrDefault(x => x.Target);

            if (target == null)
                throw new Exception(Common_lang.NOT_FOUND_DATA);

            target.CreateDateTime = DateTime.Now;
            target.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            using (var transactionscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Operator(x =>
                {
                    var context = (SMARTIIEntities)x;

                    // 先建立Target 後 , 取得ID
                    var entity = AutoMapper.Mapper.Map<HEADQUARTERS_NODE>(target);
                    context.HEADQUARTERS_NODE.Add(entity);

                    context.SaveChanges();

                    target.NodeID = entity.NODE_ID;

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
                        hnode.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;

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

                transactionscope.Complete();
            }
        }

        public async Task UpdateTree(IOrganizationNode node)
        {
            var data = (HeaderQuarterNode)node;
            var flatten = data.FlattenNSM();

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
                    hnode.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;

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
        }

        public async Task Disable(int NodeID)
        {
            var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == NodeID);
            var caseCon = new MSSQLCondition<CASE>(x => x.CASE_TYPE != (int)CaseType.Finished);


            // 找到該結點左右邊界
            var locator = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetOfSpecific(con, x => new
            {
                left = x.LEFT_BOUNDARY,
                right = x.RIGHT_BOUNDARY
            });

            // 取得底下清單
            con.Or(x => x.LEFT_BOUNDARY > locator.left && x.RIGHT_BOUNDARY < locator.right);

            var nodeIDs = _OrganizationAggregate.HeaderQuarterNode_T1_.GetListOfSpecific(con, x => x.NODE_ID).Cast<int?>();

            _CaseAggregate.Case_T1_T2_.Operator(context => 
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var unCloseCases = db.CASE.AsNoTracking().Where(x => x.CASE_TYPE != (int)CaseType.Finished).Select(x => x.CASE_ID).ToList();

                // 判斷停用的節點 是否還有未結案的案件(轉派對象/被反應者/反應者)
                if (
                    db.CASE_ASSIGNMENT_USER.Any(x => unCloseCases.Contains(x.CASE_ID) && nodeIDs.Contains(x.NODE_ID)) ||
                    db.CASE_COMPLAINED_USER.Any(x => unCloseCases.Contains(x.CASE_ID) && nodeIDs.Contains(x.NODE_ID)) ||
                    db.CASE_CONCAT_USER.Any(x => unCloseCases.Contains(x.CASE_ID) && nodeIDs.Contains(x.NODE_ID))
                )
                {
                    throw new Exception(HeaderQuarterNode_lang.HEADERQUARTER_DISABLE_ERROR);
                }

            });

            // 更新
            con.ActionModify(x =>
            {
                x.IS_ENABLED = false;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            await _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_
                           .UpdateRange(con)
                           .Async();
        }

        public async Task<IOrganizationNode> Get(int nodeID)
        {
            var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == nodeID);
            con.IncludeBy(x => x.ENTERPRISE);
            con.IncludeBy(x => x.HEADQUARTERS_NODE2);

            // 取得節點清單
            var node = await _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(con).Async();

            return node;
        }

        public async Task<IOrganizationNode> GetComplete(int nodeID)
        {
            var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == nodeID);

            con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            con.IncludeBy(x => x.ENTERPRISE);
            con.IncludeBy(x => x.HEADQUARTERS_NODE2);
            con.IncludeBy(x => x.STORE);

            var node = await _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(con).Async();

            var jcon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == node.NodeID &&
                                                         x.ORGANIZATION_TYPE == (byte)node.OrganizationType);

            jcon.IncludeBy(x => x.USER);
            jcon.IncludeBy(x => x.JOB);
            var jobs = await _OrganizationAggregate.JobPosition_T1_T2_.GetList(jcon).Async();

            node.JobPosition = jobs.ToList();

            return node;
        }

        public async Task<IEnumerable<IOrganizationNode>> GetAll()
        {
            var con = new MSSQLCondition<HEADQUARTERS_NODE>();
            con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

            return await _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_
                            .GetList(con)
                            .Async();
        }

        public async Task<IOrganizationNode> Update(IOrganizationNode node)
        {
            var data = (HeaderQuarterNode)node;

            // 取得組織定義
            var nodeDefind = await _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_
                                                         .Get(x => x.ID == node.NodeType)
                                                         .Async();

            if (nodeDefind == null)
                throw new Exception(Common_lang.UNCHOOSE_DEFIN_TREE);

            var con = new MSSQLCondition<HEADQUARTERS_NODE>(x => x.NODE_ID == node.NodeID);

            con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            con.IncludeBy(x => x.ENTERPRISE);
            con.IncludeBy(x => x.STORE);

            con.ActionModify(x =>
            {
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                x.NAME = data.Name;
                x.IS_ENABLED = data.IsEnabled;
                x.NODE_TYPE = nodeDefind.ID;
                x.NODE_TYPE_KEY = nodeDefind.Key;
            });

            using (var transactionscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_
                            .Update(con)
                            .Async();

                // 該節點停用時  也停用底下節點
                if (data.IsEnabled == false)
                {
                    await this.Disable(result.ID);
                }

                // 取得服務
                var service = _Strategies.TryGetService(nodeDefind?.Key, EssentialCache.BusinessKeyValue.COMMONBU);

                service?.WhenNodeUpdate(data);

                //需同步修改定義檔的識別名稱
                var conUnd = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(x => x.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter);
                conUnd.And(x => x.IDENTIFICATION_ID == node.NodeID);
                conUnd.ActionModify(x => x.IDENTIFICATION_NAME = node.Name);

                _OrganizationAggregate.OrganizationNodeDefinition_T1_.UpdateRange(conUnd);


                transactionscope.Complete();

                return result;
            }
        }

        public async Task AddJobs(int nodeID, int[] jobIDs)
        {
            var node = _OrganizationAggregate.HeaderQuarterNode_T1_IOrganizationNode_.Get(x => x.NODE_ID == nodeID);

            _OrganizationAggregate.JobPosition_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                var entities = context.JOB.Where(c => jobIDs.Contains(c.ID));

                foreach (var entity in entities)
                {
                    entity.NODE_JOB.Add(new NODE_JOB()
                    {
                        IDENTIFICATION_ID = ((HeaderQuarterNode)node).BUID,
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

        public async Task DeleteJob(int nodeJobID)
        {
            _OrganizationAggregate.JobPosition_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var query = db.NODE_JOB.Include("USER").AsQueryable();

                var nodeJob = query.Where(x => x.ID == nodeJobID).FirstOrDefault();

                nodeJob.USER.ToList().ForEach(user =>
                {
                    db.Entry(user).State = EntityState.Modified;
                    user.VERSION = DateTime.Now;
                });

                db.NODE_JOB.Remove(nodeJob);

                db.SaveChanges();
            });
        }

        public async Task AddUsers(int nodeJobID, string[] userIDs)
        {
            _OrganizationAggregate.JobPosition_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                var users = context.USER
                                   .Where(g => userIDs.Contains(g.USER_ID))
                                   .ToList();

                var jobPosition = context.NODE_JOB.First(g => g.ID == nodeJobID);

                users.ForEach(c =>
                {
                    jobPosition.USER.Add(c);
                    c.VERSION = DateTime.Now;
                });

                context.SaveChanges();
            });
        }

        public async Task DeleteUser(int nodeJobID, string userID)
        {
            _OrganizationAggregate.JobPosition_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                var user = context.USER
                                  .FirstOrDefault(g => g.USER_ID == userID);

                var nodeJob = context.NODE_JOB
                                     .Include("USER")
                                     .FirstOrDefault(g => g.ID == nodeJobID);

                user.VERSION = DateTime.Now;
                nodeJob.USER.Remove(user);

                context.SaveChanges();
            });
        }

        public IOrganizationalTerm GetTerm(int nodeID, OrganizationType organizationType)
        {
            // 先找到BU 識別碼
            var con = new MSSQLCondition<HEADQUARTERS_NODE>(
                            x => x.NODE_ID == nodeID &&
                            x.ORGANIZATION_TYPE == (byte)organizationType);

            var nodeKey = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(con, x => x.NODE_KEY);

           
            return new HeaderQuarterTerm(nodeID, organizationType, nodeKey);
        }

        public IOrganizationalTerm GetTerm(string ndoeKey, OrganizationType organizationType)
        {
            // 先找到BU 識別碼
            var con = new MSSQLCondition<HEADQUARTERS_NODE>(
                            x => x.NODE_KEY == ndoeKey &&
                            x.ORGANIZATION_TYPE == (byte)organizationType);

            var BuID = _OrganizationAggregate.HeaderQuarterNode_T1_.GetOfSpecific(con, x => x.NODE_ID);


            return new HeaderQuarterTerm(BuID, organizationType, ndoeKey);
        }

        public Task<bool> CheckDisableVendor(int nodeID)
        {
            throw new NotImplementedException();
        }
    }
}
