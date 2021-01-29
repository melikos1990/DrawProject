using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Nito.AsyncEx;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Service
{
    public class CaseSearchService : ICaseSearchService
    {
        private readonly ICaseFacade _CaseFacade;
        private readonly ICaseSearchFacade _CaseSearchFacade;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly ICaseSourceFacade _CaseSourceFacade;
        private readonly ICaseSearchProvider _CaseSearchProvider;
        private readonly IIndex<string, ICaseSpecificFactory> _CaseSpecificFactory;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;
        private readonly IIndex<string, IFlow> _Flows;

        private static object _Lock = new object();
        private readonly AsyncLock _mutex = new AsyncLock();

        public CaseSearchService(
                           IIndex<string, IFlow> Flows,
                           ICaseFacade CaseFacade,
                           ICaseSearchFacade CaseSearchFacade,
                           ICaseAggregate CaseAggregate,
                           IMasterAggregate MasterAggregate,
                           ICommonAggregate CommonAggregate,
                           INotificationAggregate NotificationAggregate,
                           IOrganizationAggregate OrganizationAggregate,
                           ICaseSourceFacade CaseSourceFacade,
                           ICaseSearchProvider CaseSearchProvider,
                           IIndex<string, ICaseSpecificFactory> CaseSpecificFactory,
                           HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                           QuestionClassificationResolver QuestionClassificationResolver,
                           IIndex<NotificationType, INotificationProvider> NotificationProviders)
        {
            _CaseFacade = CaseFacade;
            _CaseAggregate = CaseAggregate;
            _CaseSearchFacade = CaseSearchFacade;
            _MasterAggregate = MasterAggregate;
            _CommonAggregate = CommonAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _NotificationAggregate = NotificationAggregate;
            _CaseSourceFacade = CaseSourceFacade;
            _CaseSearchProvider = CaseSearchProvider;
            _CaseSpecificFactory = CaseSpecificFactory;
            _NotificationProviders = NotificationProviders;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
            _QuestionClassificationResolver = QuestionClassificationResolver;
            _Flows = Flows;
        }

        /// <summary>
        /// 取得案件查詢清單(客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<List<SP_GetCaseList>> GetCaseForCustomerLists(CaseCallCenterCondition condition)
        {
            var temp = new List<SP_GetCaseList>();
            var result = new List<SP_GetCaseList>();
            //找出該使用者可使用群組
            var conGroupID = new MSSQLCondition<CALLCENTER_NODE>();

            conGroupID.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);

            var user = ContextUtility.GetUserIdentity().Instance;

            conGroupID.FilterNodeFromPosition(user.JobPositions);

            conGroupID.And(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.Group);

            var group = _OrganizationAggregate.CallCenterNode_T1_T2_.GetList(conGroupID).ToList();

            var groupID = string.Join("','", group.Select(x => x.NodeID));

            using (var scope = TrancactionUtility.TransactionScopeNoLock())
            {
                //取得該分類代號底下分類ID
                if (condition.ClassificationID.HasValue)
                    condition.ClassificationIDGroup =  await _CaseSearchFacade.GetClassificationIDGroup(condition.ClassificationID.Value);
                //找出案件資料
                result = _CaseSearchFacade.GetCaseForCustomerLists(condition, groupID).ToList();

            }

            return result;
        }

        /// <summary>
        /// 案件查詢-Excel匯出 (客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExcelCaseForCustomer(CaseCallCenterCondition condition)
        {
            //取得資料
            var list = await GetCaseForCustomerLists(condition);
            var caseIDList = list.Select(y => y.CaseID);
            //商品
            var concase = new MSSQLCondition<CASE>();
            concase.IncludeBy(x => x.CASE_ITEM);
            concase.And(x => caseIDList.Contains(x.CASE_ID));
            var caseList = _CaseAggregate.Case_T1_T2_.GetList(concase);


            var conitem = new MSSQLCondition<ITEM>();
            var itemIDList = caseList.SelectMany(c => c.Items.Select(y => y.ItemID));
            conitem.And(x => itemIDList.Contains(x.ID));

            var itemlist = _MasterAggregate.Item_T1_T2_Expendo_.GetList(conitem);


            foreach (var item in list)
            {
                var caseItem = caseList.Where(y => y.CaseID == item.CaseID).FirstOrDefault().Items;
                foreach (var j in caseItem)
                {
                    j.Item = itemlist.Where(c => c.ID == j.ItemID).FirstOrDefault();
                }
                item.CaseItemList = caseItem;
            }
            //匯出EXCEL
            var result = _CaseSearchProvider.CreateCaseCustomerExcel(list, condition);

            return result;
        }

        /// <summary>
        /// 取得案件查詢清單(總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<List<SP_GetCaseList>> GetCaseForHSLists(CaseHSCondition condition)
        {
            var temp = new List<SP_GetCaseList>();
            var result = new List<SP_GetCaseList>();

            string headNodeID = null;
            //20201208 - 為true時不須帶入節點編號(BU查詢功能)
            if (!condition.IsBusinessAll)
            {
                //找出該使用者組織節點
                var conNodeID = new MSSQLCondition<HEADQUARTERS_NODE>();

                conNodeID.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
                var user = ContextUtility.GetUserIdentity().Instance;
                conNodeID.FilterNodeFromPosition(user.JobPositions);

                var headNode = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conNodeID).ToList();

                headNodeID = string.Join("','", headNode.Select(x => x.NodeID));
            }
           
            //取得該分類代號底下分類ID
            if (condition.ClassificationID.HasValue)
                condition.ClassificationIDGroup = await _CaseSearchFacade.GetClassificationIDGroup(condition.ClassificationID.Value);
            //找出案件資料
            result = _CaseSearchFacade.GetCaseHSLists(condition, headNodeID).ToList();
            return result;
        }

        /// <summary>
        /// 案件查詢-Excel匯出 (總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExcelCaseForHS(CaseHSCondition condition)
        {
            //取得資料
            var list = await GetCaseForHSLists(condition);

            var caseIDList = list.Select(y => y.CaseID);
            //商品
            var concase = new MSSQLCondition<CASE>();
            concase.IncludeBy(x => x.CASE_ITEM);
            concase.IncludeBy(x => x.CASE_ITEM.Select(y => y.ITEM));
            concase.And(x => caseIDList.Contains(x.CASE_ID));
            var caseList = _CaseAggregate.Case_T1_T2_.GetList(concase);


            var conitem = new MSSQLCondition<ITEM>();
            var itemIDList = caseList.SelectMany(c => c.Items.Select(y => y.ItemID));
            conitem.And(x => itemIDList.Contains(x.ID));

            var itemlist = _MasterAggregate.Item_T1_T2_Expendo_.GetList(conitem);

            foreach (var item in list)
            {
                var caseItem = caseList.Where(y => y.CaseID == item.CaseID).FirstOrDefault().Items;
                foreach (var j in caseItem)
                {
                    j.Item = itemlist.Where(c => c.ID == j.ItemID).FirstOrDefault();
                }
                item.CaseItemList = caseItem;
            }
            //匯出EXCEL
            var result = _CaseSearchProvider.CreateCaseHSExcel(list, condition);
            return result;
        }

        /// <summary>
        /// 取得轉派案件查詢清單(客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<List<ExcelCaseAssignmentList>> GetCaseAssignmentForCustomerLists(CaseAssignmentCallCenterCondition condition)
        {
            var result = new List<ExcelCaseAssignmentList>();
            //找出該使用者可使用群組
            var conGroupID = new MSSQLCondition<CALLCENTER_NODE>();
            conGroupID.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            var user = ContextUtility.GetUserIdentity().Instance;
            conGroupID.FilterNodeFromPosition(user.JobPositions);

            conGroupID.And(x => x.ORGANIZATION_NODE_DEFINITION.KEY == EssentialCache.NodeDefinitionValue.Group);

            var group = _OrganizationAggregate.CallCenterNode_T1_T2_.GetList(conGroupID).ToList();

            var groupID = string.Join("','", group.Select(x => x.NodeID));

            //取得該分類代號底下分類ID
            if (condition.ClassificationID.HasValue)
                condition.ClassificationIDGroup = await _CaseSearchFacade.GetClassificationIDGroup(condition.ClassificationID.Value);


            //找出轉派案件
            if (condition.AssignmentType != null)
            {
                switch (condition.AssignmentType)
                {
                    case CaseAssignmentProcessType.Notice:
                        result = _CaseSearchFacade.GetCaseAssignmentNoticeForCustomerLists(condition, groupID).ToList();
                        break;

                    case CaseAssignmentProcessType.Invoice:
                        result = _CaseSearchFacade.GetCaseAssignmentInvoiceForCustomerLists(condition, groupID).ToList();
                        break;

                    case CaseAssignmentProcessType.Assignment:
                        result = _CaseSearchFacade.GetCaseAssignmentForCustomerLists(condition, groupID).ToList();
                        break;

                    case CaseAssignmentProcessType.Communication:
                        result = _CaseSearchFacade.GetCaseAssignmentCommuncateForCustomerLists(condition, groupID).ToList();
                        break;
                }
            }
            else
            {
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentNoticeForCustomerLists(condition, groupID).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentInvoiceForCustomerLists(condition, groupID).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentForCustomerLists(condition, groupID).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentCommuncateForCustomerLists(condition, groupID).ToList());
            }
            result = result.OrderBy(x => x.CaseID).ThenBy(x => x.SN).ToList();
            return result;
        }

        /// <summary>
        /// 轉派案件查詢-Excel匯出 (客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExcelCaseAssignmentForCustomer(CaseAssignmentCallCenterCondition condition)
        {
            //取得資料
            var list = await GetCaseAssignmentForCustomerLists(condition);
            //匯出EXCEL
            var result = _CaseSearchProvider.CreateCaseAssignmentCustomerExcel(list, condition);

            return result;
        }

        /// <summary>
        /// 取得轉派案件查詢清單(總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<List<ExcelCaseAssignmentList>> GetCaseAssignmentForHSLists(CaseAssignmentHSCondition condition, OrganizationType organizationType)
        {
            var temp = new List<SP_GetCaseAssignmentList>();
            var result = new List<ExcelCaseAssignmentList>();

            string HeadNodeID = null;
            //20201208 - 為true時不須帶入節點編號(BU查詢功能)
            if (!condition.IsBusinessAll)
            {
                //找出該使用者組織節點
                var conNodeID = new MSSQLCondition<HEADQUARTERS_NODE>();

                conNodeID.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
                var user = ContextUtility.GetUserIdentity().Instance;
                conNodeID.FilterNodeFromPosition(user.JobPositions);
                var HeadNode = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conNodeID).ToList();
                HeadNodeID = string.Join("','", HeadNode.Select(x => x.NodeID));
            }

            //取得該分類代號底下分類ID
            if (condition.ClassificationID.HasValue)
                condition.ClassificationIDGroup = await _CaseSearchFacade.GetClassificationIDGroup(condition.ClassificationID.Value);
            //找出轉派案件
            if (condition.AssignmentType != null)
            {
                switch (condition.AssignmentType)
                {
                    case CaseAssignmentProcessType.Notice:
                        result = _CaseSearchFacade.GetCaseAssignmentNoticeForHSLists(condition, HeadNodeID, organizationType).ToList();
                        break;

                    case CaseAssignmentProcessType.Invoice:
                        result = _CaseSearchFacade.GetCaseAssignmentInvoiceForHSLists(condition, HeadNodeID, organizationType).ToList();
                        break;

                    case CaseAssignmentProcessType.Assignment:
                        result = _CaseSearchFacade.GetCaseAssignmentForHSLists(condition, HeadNodeID, organizationType).ToList();
                        break;

                    case CaseAssignmentProcessType.Communication:
                        result = _CaseSearchFacade.GetCaseAssignmentCommuncateForHSLists(condition, HeadNodeID, organizationType).ToList();
                        break;
                }
            }
            else
            {
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentNoticeForHSLists(condition, HeadNodeID, organizationType).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentInvoiceForHSLists(condition, HeadNodeID, organizationType).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentForHSLists(condition, HeadNodeID, organizationType).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentCommuncateForHSLists(condition, HeadNodeID, organizationType).ToList());
            }
            result = result.OrderBy(x => x.CaseID).ThenBy(x => x.SN).ToList();
            return result;
        }

        /// <summary>
        /// 轉派案件查詢-Excel匯出 (總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExcelCaseAssignmentForHS(CaseAssignmentHSCondition condition, OrganizationType organizationType)
        {
            //取得資料
            var list = await GetCaseAssignmentForHSLists(condition, organizationType);
            //匯出EXCEL
            var result = _CaseSearchProvider.CreateCaseAssignmentHSExcel(list, condition);

            return result;
        }

        /// <summary>
        /// 取得轉派案件查詢清單(廠商)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<List<ExcelCaseAssignmentList>> GetCaseAssignmentForVendorLists(CaseAssignmentHSCondition condition, OrganizationType organizationType)
        {
            var temp = new List<SP_GetCaseAssignmentList>();
            var result = new List<ExcelCaseAssignmentList>();
            //找出該使用者組織節點
            var conNodeID = new MSSQLCondition<VENDOR_NODE>();

            conNodeID.IncludeBy(x => x.ORGANIZATION_NODE_DEFINITION);
            var user = ContextUtility.GetUserIdentity().Instance;
            conNodeID.FilterNodeFromPosition(user.JobPositions);
            var efe = user.JobPositions.Select(x => x.NodeID);
            var VendorNode = _OrganizationAggregate.VendorNode_T1_T2_.GetList(conNodeID).ToList();
            var VendorNodeID = string.Join("','", VendorNode.Select(x => x.NodeID));

            //取得該分類代號底下分類ID
            if (condition.ClassificationID.HasValue)
                condition.ClassificationIDGroup = await _CaseSearchFacade.GetClassificationIDGroup(condition.ClassificationID.Value);
            //找出轉派案件
            if (condition.AssignmentType != null)
            {
                switch (condition.AssignmentType)
                {
                    case CaseAssignmentProcessType.Notice:
                        result = _CaseSearchFacade.GetCaseAssignmentNoticeForHSLists(condition, VendorNodeID, organizationType).ToList();
                        break;

                    case CaseAssignmentProcessType.Invoice:
                        result = _CaseSearchFacade.GetCaseAssignmentInvoiceForHSLists(condition, VendorNodeID, organizationType).ToList();
                        break;

                    case CaseAssignmentProcessType.Assignment:
                        result = _CaseSearchFacade.GetCaseAssignmentForHSLists(condition, VendorNodeID, organizationType).ToList();
                        break;

                    case CaseAssignmentProcessType.Communication:
                        result = _CaseSearchFacade.GetCaseAssignmentCommuncateForHSLists(condition, VendorNodeID, organizationType).ToList();
                        break;
                }
            }
            else
            {
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentNoticeForHSLists(condition, VendorNodeID, organizationType).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentInvoiceForHSLists(condition, VendorNodeID, organizationType).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentForHSLists(condition, VendorNodeID, organizationType).ToList());
                result.AddRange(_CaseSearchFacade.GetCaseAssignmentCommuncateForHSLists(condition, VendorNodeID, organizationType).ToList());
            }
            result = result.OrderBy(x => x.CaseID).ThenBy(x => x.SN).ToList();
            return result;
        }

        /// <summary>
        /// 轉派案件查詢-Excel匯出 (總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<Byte[]> ExcelCaseAssignmentForVendor(CaseAssignmentHSCondition condition, OrganizationType organizationType)
        {
            //取得資料
            var list = await GetCaseAssignmentForVendorLists(condition, organizationType);
            //匯出EXCEL
            var result = _CaseSearchProvider.CreateCaseAssignmentVendorExcel(list, condition);

            return result;
        }
    }
}
