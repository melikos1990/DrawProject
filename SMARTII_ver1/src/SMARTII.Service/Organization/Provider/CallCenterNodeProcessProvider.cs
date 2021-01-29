﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Autofac.Features.Indexed;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Provider
{
    public class CallCenterNodeProcessProvider : IOrganizationNodeProcessProvider
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, IOrganizationProcessStrategy> _Strategies;

        public CallCenterNodeProcessProvider(IOrganizationAggregate OrganizationAggregate,
                                        IIndex<string, IOrganizationProcessStrategy> Strategies)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _Strategies = Strategies;
        }

        /// <summary>
        /// 新增節點職稱(多筆)
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="jobIDs"></param>
        /// <returns></returns>
        public async Task AddJobs(int nodeID, int[] jobIDs)
        {
            var node = _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_.Get(x => x.NODE_ID == nodeID);

            _OrganizationAggregate.JobPosition_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                var entities = context.JOB.Where(c => jobIDs.Contains(c.ID));

                foreach (var entity in entities)
                {
                    entity.NODE_JOB.Add(new NODE_JOB()
                    {
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

        /// <summary>
        /// 刪除節點職稱(單筆)
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 新增節點職稱人員(多筆)
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 刪除節點職稱人員(單筆)
        /// </summary>
        /// <param name="nodeJobID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 新增節點
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task Create(IOrganizationNode node)
        {
            var data = (CallCenterNode)node;
            var flatten = data.FlattenNSM();

            var target = flatten.FirstOrDefault(x => x.Target);

            if (target == null)
                throw new Exception(Common_lang.NOT_FOUND_DATA);

            target.CreateDateTime = DateTime.Now;
            target.CreateUserName = ContextUtility.GetUserIdentity()?.Name;

            using (var transactionscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _OrganizationAggregate.CallCenterNode_T1_T2_.Operator(x =>
                {
                    var context = (SMARTIIEntities)x;

                    // 先建立Target 後 , 取得ID
                    var entity = AutoMapper.Mapper.Map<CALLCENTER_NODE>(target);
                    context.CALLCENTER_NODE.Add(entity);

                    context.SaveChanges();

                    target.NodeID = entity.NODE_ID;

                    // 組織結點更新
                    foreach (var item in flatten)
                    {
                        var hnode = context.CALLCENTER_NODE
                                           .First(g => g.NODE_ID == item.NodeID);

                        hnode.LEFT_BOUNDARY = item.LeftBoundary;
                        hnode.RIGHT_BOUNDARY = item.RightBoundary;
                        hnode.DEPTH_LEVEL = item.Level;
                        hnode.PARENT_ID = item.ParentLocator;
                        hnode.PARENT_PATH = item.ParentPath;
                        hnode.UPDATE_DATETIME = DateTime.Now;
                        hnode.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;

                        var nodeJobs = context.NODE_JOB
                                             .Where(c => c.NODE_ID == hnode.NODE_ID &&
                                                         c.ORGANIZATION_TYPE == hnode.ORGANIZATION_TYPE);

                        foreach (var jobEntity in nodeJobs)
                        {
                            jobEntity.LEFT_BOUNDARY = item.LeftBoundary;
                            jobEntity.RIGHT_BOUNDARY = item.RightBoundary;
                        }
                    }
                    context.SaveChanges();
                });

                transactionscope.Complete();
            }
        }

        /// <summary>
        /// 節點上停用
        /// </summary>
        /// <param name="NodeID"></param>
        /// <returns></returns>
        public async Task Disable(int NodeID)
        {
            var con = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == NodeID);

            // 找到該結點左右邊界
            var locator = _OrganizationAggregate.CallCenterNode_T1_T2_.GetOfSpecific(con, x => new
            {
                left = x.LEFT_BOUNDARY,
                right = x.RIGHT_BOUNDARY
            });

            // 取得底下清單
            con.Or(x => x.LEFT_BOUNDARY > locator.left && x.RIGHT_BOUNDARY < locator.right);

            // 更新
            con.ActionModify(x =>
            {
                x.IS_ENABLED = false;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });

            await _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_
                           .UpdateRange(con)
                           .Async();
        }

        /// <summary>
        /// 取得節點資訊
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public async Task<IOrganizationNode> GetComplete(int nodeID)
        {
            var con = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == nodeID);

            con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            con.IncludeBy(x => x.CALLCENTER_NODE2);
            con.IncludeBy(x => x.HEADQUARTERS_NODE);

            var node = await _OrganizationAggregate.CallCenterNode_T1_T2_.Get(con).Async();

            var jcon = new MSSQLCondition<NODE_JOB>(x => x.NODE_ID == node.NodeID &&
                                                         x.ORGANIZATION_TYPE == (byte)node.OrganizationType);

            jcon.IncludeBy(x => x.USER);
            jcon.IncludeBy(x => x.JOB);
            var jobs = await _OrganizationAggregate.JobPosition_T1_T2_.GetList(jcon).Async();

            node.JobPosition = jobs.ToList();

            return node;
        }

        /// <summary>
        /// 取得組織樹
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IOrganizationNode>> GetAll()
        {
            var con = new MSSQLCondition<CALLCENTER_NODE>();
            con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            con.IncludeBy(x => x.CALLCENTER_NODE2);

            return await _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_
                            .GetList(con)
                            .Async();
        }

        /// <summary>
        /// 修改節點基本資訊
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task<IOrganizationNode> Update(IOrganizationNode node)
        {
            var data = (CallCenterNode)node;

            // 取得組織定義
            var nodeDefind = await _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_
                                                         .Get(x => x.ID == node.NodeType)
                                                         .Async();

            if (nodeDefind == null)
                throw new Exception(Common_lang.UNCHOOSE_DEFIN_TREE);

            var con = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == data.NodeID);

            var ccNode = _OrganizationAggregate.CallCenterNode_T1_T2_.Get(con);
            
            con.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            con.IncludeBy(x => x.HEADQUARTERS_NODE);

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

                // 組織定義不同時 移除與總部組織樹的關聯和節點職稱
                if (ccNode.NodeType != nodeDefind.ID)
                {

                    _OrganizationAggregate.JobPosition_T1_T2_.Operator(context =>
                    {
                        var db = (SMARTIIEntities)context;
                        db.Configuration.LazyLoadingEnabled = false;
                        
                        var nodeJobs = db.NODE_JOB.AsQueryable()
                                                  .Include(x => x.USER)
                                                  .Where(x => x.NODE_ID == ccNode.NodeID && 
                                                              x.ORGANIZATION_TYPE == (byte)OrganizationType.CallCenter) 
                                                  .ToList();

                        var users = nodeJobs.SelectMany(x => x.USER).DistinctBy(x => x.USER_ID);

                        users.ForEach(user => user.VERSION = DateTime.Now);
                        db.NODE_JOB.RemoveRange(nodeJobs);

                        db.SaveChanges();
                    });

                    con.ActionModify(x => x.HEADQUARTERS_NODE = null);
                }

                var result = await _OrganizationAggregate.CallCenterNode_T1_IOrganizationNode_
                            .Update(con, ctx =>
                            {
                                (ctx as SMARTIIEntities).Configuration.ProxyCreationEnabled = true;
                                (ctx as SMARTIIEntities).Configuration.LazyLoadingEnabled = true;
                            })
                            .Async();


                // 取得服務
                var service = _Strategies.TryGetService(nodeDefind?.Key);

                service?.WhenNodeUpdate(data);

                // 該節點停用時  也停用底下節點
                if (data.IsEnabled == false)
                {
                    await this.Disable(result.ID);
                }

                //需同步修改定義檔的識別名稱
                var conUnd = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(x => x.ORGANIZATION_TYPE == (int)OrganizationType.CallCenter);
                conUnd.And(x => x.IDENTIFICATION_ID == node.NodeID);
                conUnd.ActionModify(x => x.IDENTIFICATION_NAME = node.Name);

                _OrganizationAggregate.OrganizationNodeDefinition_T1_.UpdateRange(conUnd);

                transactionscope.Complete();

                return result;
            }
        }

        /// <summary>
        /// 拖拉組織樹
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public async Task UpdateTree(IOrganizationNode node)
        {
            var data = (CallCenterNode)node;
            var flatten = data.FlattenNSM();

            _OrganizationAggregate.CallCenterNode_T1_T2_.Operator(x =>
            {
                var context = (SMARTIIEntities)x;

                // 組織結點更新
                foreach (var item in flatten)
                {
                    var hnode = context.CALLCENTER_NODE
                                       .First(g => g.NODE_ID == item.NodeID);

                    hnode.LEFT_BOUNDARY = item.LeftBoundary;
                    hnode.RIGHT_BOUNDARY = item.RightBoundary;
                    hnode.DEPTH_LEVEL = item.Level;
                    hnode.PARENT_ID = item.ParentLocator;
                    hnode.PARENT_PATH = item.ParentPath;
                    hnode.UPDATE_DATETIME = DateTime.Now;
                    hnode.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
                    hnode.CALLCENTER_ID = item.CallCenterID;

                    var nodeJob = context.NODE_JOB
                                         .Where(c => c.NODE_ID == hnode.NODE_ID &&
                                                     c.ORGANIZATION_TYPE == hnode.ORGANIZATION_TYPE);

                    foreach (var jobEntity in nodeJob)
                    {
                        jobEntity.IDENTIFICATION_ID = item.CallCenterID;
                        jobEntity.LEFT_BOUNDARY = item.LeftBoundary;
                        jobEntity.RIGHT_BOUNDARY = item.RightBoundary;
                    }
                }
                context.SaveChanges();
            });
        }

        /// <summary>
        /// 登入時，取得CC組織人員設定
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public async Task<IOrganizationNode> Get(int nodeID)
        {
            var con = new MSSQLCondition<CALLCENTER_NODE>(x => x.NODE_ID == nodeID);
            con.IncludeBy(x => x.HEADQUARTERS_NODE);
            con.IncludeBy(x => x.CALLCENTER_NODE2);
            var node = await _OrganizationAggregate.CallCenterNode_T1_T2_.Get(con).Async();

            return node;
        }

        public Task<bool> CheckDisableVendor(int nodeID)
        {
            throw new NotImplementedException();
        }
    }
}
