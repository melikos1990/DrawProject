using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.DI;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Organization.Facade
{
    public class NodeDefinitionFacade : INodeDefinitionFacade
    {
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, IOrganizationProcessStrategy> _Strategies;

        public NodeDefinitionFacade(IOrganizationAggregate OrganizationAggregate,
                                    IIndex<string, IOrganizationProcessStrategy> Strategies)
        {
            _OrganizationAggregate = OrganizationAggregate;
            _Strategies = Strategies;
        }

        /// <summary>
        /// 單一更新定義
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Update(OrganizationNodeDefinition data)
        {

            var isRepeat = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.HasAny(x => x.ORGANIZATION_TYPE == (byte)data.OrganizationType
                                                                            && x.IDENTIFICATION_ID == data.Identification
                                                                            && x.NAME == data.Name
                                                                            && x.ID != data.ID);
            if (isRepeat)
                throw new Exception(NodeDefinition_lang.ORGANIZATION_NODE_DEFINITION_NAME_REPEAT);

            var con = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(
                x => x.ID == data.ID &&
                x.ORGANIZATION_TYPE == (byte)data.OrganizationType);

            con.IncludeBy(x => x.JOB);

            con.ActionModify(x =>
            {
                x.LEVEL = data.Level;
                x.ORGANIZATION_TYPE = (byte)data.OrganizationType;
                x.IS_ENABLED = data.IsEnabled;
                x.NAME = data.Name;
                x.UPDATE_DATETIME = data.UpdateDateTime;
                x.UPDATE_USERNAME = data.UpdateUserName;
                x.IDENTIFICATION_NAME = data.IdentificationName;
                x.IDENTIFICATION_ID = data.Identification;
                x.KEY = data.Key;
            });

            using (var scope = TrancactionUtility.TransactionScope())
            {
                _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.Update(con);

                //有停用再將該定義底下職稱拔除關聯
                if (!data.IsEnabled)
                {
                    var nodeDefind = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.Get(con);
                    removeNodeBuRelated(nodeDefind);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 單一停用定義
        /// </summary>
        /// <param name="organizationType"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task Disable(OrganizationType organizationType, int ID)
        {

            var nowTime = DateTime.Now;

            var con = new MSSQLCondition<ORGANIZATION_NODE_DEFINITION>(
                x => x.ID == ID &&
                x.ORGANIZATION_TYPE == (byte)organizationType);

            con.IncludeBy(x => x.JOB);

            con.ActionModify(x =>
            {
                x.IS_ENABLED = false;
                x.UPDATE_DATETIME = nowTime;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity()?.Name;
            });
            using (var scope = TrancactionUtility.TransactionScope())
            {
                _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.Update(con);

                //停用該定義底下職稱
                var nodeDefind = _OrganizationAggregate.OrganizationNodeDefinition_T1_T2_.Get(con);
                removeNodeBuRelated(nodeDefind);

                scope.Complete();
            }
        }

        /// <summary>
        /// 單一更新定義職稱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Job> UpdateJob(Job data)
        {
            var isRepeat = _OrganizationAggregate.Job_T1_T2_.HasAny(x => x.ORGANIZATION_TYPE == (byte)data.OrganizationType
                                                            && x.DEFINITION_ID == data.DefinitionID
                                                            && x.NAME == data.Name
                                                            && x.ID != data.ID);
            if (isRepeat)
                throw new Exception(Job_lang.JOB_NAME_REPEAT);

            var con = new MSSQLCondition<JOB>(x => x.ID == data.ID);

            con.ActionModify(x =>
            {
                x.IS_ENABLED = data.IsEnabled;
                x.NAME = data.Name;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                x.LEVEL = data.Level;
                x.KEY = data.Key;
            });

            var result = new Job();

            using (var scope = TrancactionUtility.TransactionScope())
            {
                result = _OrganizationAggregate.Job_T1_T2_.Update(con);

                //有停用，才拔除關聯
                if (!data.IsEnabled)
                    removeNodeJobRelated(data.ID);

                scope.Complete();
            }

            return result;
        }

        /// <summary>
        /// 單一停用定義職稱
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task DisableJob(int ID)
        {
            var con = new MSSQLCondition<JOB>(x => x.ID == ID);

            con.ActionModify(x =>
            {
                x.IS_ENABLED = false;
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
            });

            using (var scope = TrancactionUtility.TransactionScope())
            {
                _OrganizationAggregate.Job_T1_T2_.Update(con);

                removeNodeJobRelated(ID);

                scope.Complete();
            }
        }

        /// <summary>
        /// 移除定義與組織BU的關聯
        /// </summary>
        /// <param name="nodeDefind"></param>
        private void removeNodeBuRelated(OrganizationNodeDefinition nodeDefind)
        {
            nodeDefind.Jobs.ForEach(async x =>
            {
                await DisableJob(x.ID);
            });

            //停用定義為廠商
            if (nodeDefind.Key == EssentialCache.NodeDefinitionValue.VendorGroup)
            {
                var nodeVendorList = _OrganizationAggregate.VendorNode_T1_T2_.GetList(x => x.NODE_TYPE == nodeDefind.ID).ToList();
                nodeVendorList.ForEach(x =>
                {
                    // 取得服務
                    var service = _Strategies.TryGetService(nodeDefind?.Key);

                    service?.WhenNodeUpdate(x);
                });
            }

            //停用定義為服務群組
            if (nodeDefind.Key == EssentialCache.NodeDefinitionValue.Group)
            {
                var nodeCCList = _OrganizationAggregate.CallCenterNode_T1_T2_.GetList(x => x.NODE_TYPE == nodeDefind.ID).ToList();
                nodeCCList.ForEach(x =>
                {
                    // 取得服務
                    var service = _Strategies.TryGetService(nodeDefind?.Key);

                    service?.WhenNodeUpdate(x);
                });
            }
        }

        /// <summary>
        /// 移除定義職稱與組織職稱關聯
        /// </summary>
        /// <param name="ID"></param>
        private void removeNodeJobRelated(int ID)
        {
            _OrganizationAggregate.JobPosition_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;
                db.Configuration.LazyLoadingEnabled = false;

                var query = db.NODE_JOB
                              .Include("USER")
                              .Where(x => x.JOB_ID == ID).ToList();

                query.ForEach(nodeJob =>
                {
                    nodeJob.USER.ToList().ForEach(user =>
                    {
                        db.Entry(user).State = EntityState.Modified;
                        user.VERSION = DateTime.Now;
                    });


                    db.NODE_JOB.Remove(nodeJob);
                });

                db.SaveChanges();
            });

        }
    }
}
