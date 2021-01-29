using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using LinqKit;
using Ptc.Data.Condition2.Common.Class;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Domain.Security;
using SMARTII.Domain.Thread;
using SMARTII.Service.Case.Parser;
using SMARTII.Service.Master.Resolver;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Service.Case.Facade
{
    public class CaseSearchFacade : ICaseSearchFacade
    {

        private readonly IIndex<string, IFlow> _Flows;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IIndex<string, ICaseAssignmentHSIntegrationData> _CaseAssignmentHSIntegrationData;
        private readonly IIndex<string, ICaseAssignmentCallCenterIntegrationData> _CaseAssignmentIntegrationData;
        private readonly IIndex<string, ICaseAssignmentVendorIntegrationData> _CaseAssignmentVendorIntegrationData;
        private readonly QuestionClassificationResolver _QuestionClassificationResolver;

        public CaseSearchFacade(IIndex<string, IFlow> Flows,
                                ICaseAggregate CaseAggregate,
                                IMasterAggregate MasterAggregate,
                                IOrganizationAggregate OrganizationAggregate,
                                IIndex<string, ICaseAssignmentHSIntegrationData> CaseAssignmentHSIntegrationData,
                                IIndex<string, ICaseAssignmentCallCenterIntegrationData> CaseAssignmentIntegrationData,
                                IIndex<string, ICaseAssignmentVendorIntegrationData> CaseAssignmentVendorIntegrationData,
                                QuestionClassificationResolver QuestionClassificationResolver)
        {
            _Flows = Flows;
            _CaseAggregate = CaseAggregate;
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _CaseAssignmentIntegrationData = CaseAssignmentIntegrationData;
            _CaseAssignmentHSIntegrationData = CaseAssignmentHSIntegrationData;
            _CaseAssignmentVendorIntegrationData = CaseAssignmentVendorIntegrationData;
            _QuestionClassificationResolver = QuestionClassificationResolver;

        }

        /// <summary>
        /// 取得案件查詢清單(客服)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<SP_GetCaseList> GetCaseForCustomerLists(CaseCallCenterCondition condition, string groupID)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_CASE_SEARCH_CALLCENTER(
                condition.NodeID,
                condition.CaseSourceType,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.CaseContent,
                condition.FinishContent,
                condition.ApplyUserID,
                condition.ExpectDateStarTime,
                condition.ExpectDateEndTime,
                condition.CaseWarningID,
                condition.CaseType,
                condition.ClassificationIDGroup,
                groupID
                ).ToList();

            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseList>>(list);

            //取得其他必要資料
            var tempData = CaseSearchMoreData(temp);


            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }

            var result = new List<SP_GetCaseList>();
            //篩選連絡者 被反應者
            var con = new MSSQLCondition<SP_GetCaseList>(condition);
            result.AddRange(tempData.Query(con));

            return result;
        }

        /// <summary>
        /// 取得案件查詢清單(總部、門市)SP
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<SP_GetCaseList> GetCaseHSLists(CaseHSCondition condition, string HeadNodeID)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_CASE_SEARCH_HEADQUARTER_STORE(
                condition.NodeID,
                condition.CaseSourceType,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.CaseContent,
                condition.FinishContent,
                condition.CaseWarningID,
                condition.CaseType,
                condition.ClassificationIDGroup,
                HeadNodeID
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseList>>(list);

            //取得其他必要資料
            var tempData = CaseSearchMoreData(temp);

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            var result = new List<SP_GetCaseList>();
            //篩選連絡者 被反應者
            var con = new MSSQLCondition<SP_GetCaseList>(condition);
            result.AddRange(tempData.Query(con));

            return result;
        }

        /// <summary>
        /// 取得轉派案件查詢清單(客服)SP
        /// 派工
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_SEARCH_CALLCENTER(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                GroupID
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentList>>(list);

            var caseIDList = temp.Select(y => y.CaseID);
            var caseList = new List<Domain.Case.Case>();
            var _lock = new object();

            _CaseAggregate.Case_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                db.Configuration.LazyLoadingEnabled = false;

                // 取得 1 => ** 資料
                var query = db.CASE
                                   .AsQueryable()
                                   .AsNoTracking()
                                    .Include(x => x.CASE_CONCAT_USER)
                                    .Include(x => x.CASE_COMPLAINED_USER)
                                    .Include(x => x.CASE_TAG)
                                    .Include(x => x.CASE_FINISH_REASON_DATA)
                                    .Include(x => x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION))
                                    .Include(x => x.CASE_ASSIGNMENT)
                                    .Include(x => x.CASE_ASSIGNMENT.Select(c => c.CASE_ASSIGNMENT_USER))
                                    .Include(x => x.CASE_ITEM)
                                    .Include(x => x.CASE_ITEM.Select(c => c.ITEM))
                                    ;

                // 減少查詢欄位
                var entity = query.Where(x => caseIDList.Contains(x.CASE_ID))
                                   .Select(x => new
                                   {
                                       x.CASE_ID,
                                       x.QUESION_CLASSIFICATION_ID,
                                       x.J_CONTENT,

                                       x.CASE_COMPLAINED_USER,
                                       x.CASE_CONCAT_USER,
                                       x.CASE_TAG,
                                       x.CASE_FINISH_REASON_DATA,
                                       CASE_FINISH_REASON_CLASSIFICATION = x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION),
                                       x.CASE_ASSIGNMENT,
                                       CASE_ASSIGNMENT_USER = x.CASE_ASSIGNMENT.SelectMany(g => g.CASE_ASSIGNMENT_USER),
                                       x.CASE_ITEM,
                                       ITEM = x.CASE_ITEM.Select(c => c.ITEM)
                                   }).ToList();



                // 自己 Mapping 加快速度
                Parallel.ForEach(entity, @case =>
                {
                    var _case = new Domain.Case.Case
                    {

                        CaseID = @case.CASE_ID,
                        QuestionClassificationID = @case.QUESION_CLASSIFICATION_ID,

                        CaseComplainedUsers = @case.CASE_COMPLAINED_USER.Select(x => new CaseComplainedUser
                        {
                            ID = x.ID,
                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            CaseComplainedUserType = (CaseComplainedUserType)x.COMPLAINED_USER_TYPE,
                            OwnerUserName = x.OWNER_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            SupervisorUserName = x.SUPERVISOR_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            OwnerUserPhone = x.OWNER_USER_PHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                        }).ToList(),

                        CaseConcatUsers = @case.CASE_CONCAT_USER.Select(x => new CaseConcatUser
                        {
                            ID = x.ID,
                            Mobile = x.MOBILE,
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            TelephoneBak = x.TELEPHONE_BAK.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Email = x.EMAIL.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Address = x.ADDRESS.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),

                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            UserName = x.USER_NAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Gender = x.GENDER.HasValue ? (GenderType)x.GENDER : default(GenderType?)
                        }).ToList(),

                        CaseTags = @case.CASE_TAG.Select(g => new CaseTag { ID = g.ID, Name = g.NAME }).ToList(),

                        CaseFinishReasonDatas = @case.CASE_FINISH_REASON_DATA.Select(x => new CaseFinishReasonData
                        {
                            ID = x.ID,
                            Text = x.TEXT,
                            CaseFinishReasonClassification = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID) == null ? null : new CaseFinishReasonClassification
                            {
                                Title = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID).FirstOrDefault().TITLE
                            }
                        }).ToList(),

                        CaseAssignments = @case.CASE_ASSIGNMENT.Select(x => new CaseAssignment
                        {
                            CaseID = x.CASE_ID,
                            AssignmentID = x.ASSIGNMENT_ID,
                            CaseAssignmentUsers = @case.CASE_ASSIGNMENT_USER.Where(g => g.ASSIGNMENT_ID == x.ASSIGNMENT_ID && g.CASE_ID == x.CASE_ID).Select(c => new CaseAssignmentUser
                            {
                                ID = c.ID,
                                NodeID = c.NODE_ID,
                                NodeName = c.NODE_NAME,
                                UserName = c.USER_NAME,
                                CaseComplainedUserType = (CaseComplainedUserType)c.COMPLAINED_USER_TYPE,
                                OrganizationType = c.ORGANIZATION_TYPE.HasValue ? (OrganizationType)c.ORGANIZATION_TYPE : default(OrganizationType?),
                                IsApply = c.IS_APPLY
                            }).ToList()

                        }).ToList(),

                        Items = @case.CASE_ITEM.Select(x => new CaseItem
                        {
                            ItemID = x.ITEM_ID,
                            JContent = x.JCONTENT,
                            Item = @case.ITEM.Where(c => c.ID == x.ITEM_ID) == null ? null : new Item<ExpandoObject>
                            {
                                Code = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().CODE,
                                Name = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().NAME,
                                JContent = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().J_CONTENT,
                            }
                        }).ToList(),

                        JContent = @case.J_CONTENT
                    };

                    lock (_lock)
                    {
                        caseList.Add(_case);
                    }
                    
                });

                
            });

            // 回填資料
            Parallel.ForEach(temp, item =>
            {
                var @case = caseList.FirstOrDefault(y => y.CaseID == item.CaseID);

                item.JContent = @case.JContent;
                item.CaseConcatUsersList = @case?.CaseConcatUsers?.ToList() ?? new List<CaseConcatUser>();
                item.CaseComplainedUsersList = @case?.CaseComplainedUsers?.ToList() ?? new List<CaseComplainedUser>();
                item.CaseTagList = @case?.CaseTags;
                item.CaseFinishReasonDatas = @case?.CaseFinishReasonDatas;
                item.CaseAssignmentUsersList = @case.CaseAssignments.Where(y => y.AssignmentID == item.AssignmentID && y.CaseID == item.CaseID).FirstOrDefault().CaseAssignmentUsers;
                //商品
                item.CaseItemList = @case?.Items;
            });
            

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }

            var data = new List<SP_GetCaseAssignmentList>();
            //篩選連絡者 被反應者、轉派對象
            var con = new MSSQLCondition<SP_GetCaseAssignmentList>(condition);
            data.AddRange(temp.Query(con));

            var result = _CaseAssignmentIntegrationData[nameof(CaseAssignmentCallCenterData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(客服)SP
        /// 反應單
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentInvoiceForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_COMPLAINT_INVOICE_CALLCENTER(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                GroupID
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentList>>(list);
            
            //取得其他必要資料
            var tempData = CaseAssignmentSearchMoreData(temp);

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }

            var data = new List<SP_GetCaseAssignmentList>();
            //篩選連絡者 被反應者、反應單號、反應類別
            var con = new MSSQLCondition<SP_GetCaseAssignmentList>(condition);
            data.AddRange(tempData.Query(con));
            var result = _CaseAssignmentIntegrationData[nameof(CaseAssignmentCallCenterInvoiceData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(客服)SP
        /// 通知單
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentNoticeForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_COMPLAINT_NOTICE_CALLCENTER(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                GroupID
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentList>>(list);

            //取得其他必要資料
            var tempData = CaseAssignmentSearchMoreData(temp);


            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }

            var data = new List<SP_GetCaseAssignmentList>();
            //篩選連絡者 被反應者
            var con = new MSSQLCondition<SP_GetCaseAssignmentList>(condition);
            data.AddRange(tempData.Query(con));

            var result = _CaseAssignmentIntegrationData[nameof(CaseAssignmentCallCenterNoticeData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(客服)SP
        /// 聯絡紀錄
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentCommuncateForCustomerLists(CaseAssignmentCallCenterCondition condition, string GroupID)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_COMPLAINT_COMMUNICATE_CALLCENTER(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                GroupID
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentList>>(list);

            //取得其他必要資料
            var tempData = CaseAssignmentSearchMoreData(temp);

            var data = new List<SP_GetCaseAssignmentList>();
            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            //篩選連絡者 被反應者
            var con = new MSSQLCondition<SP_GetCaseAssignmentList>(condition);
            data.AddRange(tempData.Query(con));

            var result = _CaseAssignmentIntegrationData[nameof(CaseAssignmentCallCenterCommuncateData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(總部、門市)SP
        /// 派工
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_SEARCH_HEADQUARTER_STORE(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                headNodeID,
                (byte)organizationType
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentHSList>>(list);

            
            var caseIDList = temp.Select(y => y.CaseID).Distinct();
            var caseList = new List<Domain.Case.Case>();
            var _lock = new object();

            _CaseAggregate.Case_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                db.Configuration.LazyLoadingEnabled = false;

                // 取得 1 => ** 資料
                var query = db.CASE
                                   .AsQueryable()
                                   .AsNoTracking()
                                    .Include(x => x.CASE_CONCAT_USER)
                                    .Include(x => x.CASE_COMPLAINED_USER)
                                    .Include(x => x.CASE_TAG)
                                    .Include(x => x.CASE_FINISH_REASON_DATA)
                                    .Include(x => x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION))
                                    .Include(x => x.CASE_ASSIGNMENT)
                                    .Include(x => x.CASE_ASSIGNMENT.Select(c => c.CASE_ASSIGNMENT_USER))
                                    .Include(x => x.CASE_ITEM)
                                    .Include(x => x.CASE_ITEM.Select(c => c.ITEM))
                                    ;

                // 減少查詢欄位
                var entity = query.Where(x => caseIDList.Contains(x.CASE_ID))
                                   .Select(x => new
                                   {
                                       x.CASE_ID,
                                       x.QUESION_CLASSIFICATION_ID,
                                       x.J_CONTENT,

                                       x.CASE_COMPLAINED_USER,
                                       x.CASE_CONCAT_USER,
                                       x.CASE_TAG,
                                       x.CASE_FINISH_REASON_DATA,
                                       CASE_FINISH_REASON_CLASSIFICATION = x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION),
                                       x.CASE_ASSIGNMENT,
                                       CASE_ASSIGNMENT_USER = x.CASE_ASSIGNMENT.SelectMany(g => g.CASE_ASSIGNMENT_USER),
                                       x.CASE_ITEM,
                                       ITEM = x.CASE_ITEM.Select(c => c.ITEM)
                                   }).ToList();



                // 自己 Mapping 加快速度
                Parallel.ForEach(entity, @case =>
                {
                    var _case = new Domain.Case.Case
                    {

                        CaseID = @case.CASE_ID,
                        QuestionClassificationID = @case.QUESION_CLASSIFICATION_ID,

                        CaseComplainedUsers = @case.CASE_COMPLAINED_USER.Select(x => new CaseComplainedUser
                        {
                            ID = x.ID,
                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            CaseComplainedUserType = (CaseComplainedUserType)x.COMPLAINED_USER_TYPE,
                            OwnerUserName = x.OWNER_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            SupervisorUserName = x.SUPERVISOR_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            OwnerUserPhone = x.OWNER_USER_PHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                        }).ToList(),

                        CaseConcatUsers = @case.CASE_CONCAT_USER.Select(x => new CaseConcatUser
                        {
                            ID = x.ID,
                            Mobile = x.MOBILE,
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            TelephoneBak = x.TELEPHONE_BAK.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Email = x.EMAIL.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Address = x.ADDRESS.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),

                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            UserName = x.USER_NAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Gender = x.GENDER.HasValue ? (GenderType)x.GENDER : default(GenderType?)
                        }).ToList(),

                        CaseTags = @case.CASE_TAG.Select(g => new CaseTag { ID = g.ID, Name = g.NAME }).ToList(),

                        CaseFinishReasonDatas = @case.CASE_FINISH_REASON_DATA.Select(x => new CaseFinishReasonData
                        {
                            ID = x.ID,
                            Text = x.TEXT,
                            CaseFinishReasonClassification = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID) == null ? null : new CaseFinishReasonClassification
                            {
                                Title = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID).FirstOrDefault().TITLE
                            }
                        }).ToList(),

                        CaseAssignments = @case.CASE_ASSIGNMENT?
                            .Select(x => new CaseAssignment
                            {
                                CaseID = x.CASE_ID,
                                AssignmentID = x.ASSIGNMENT_ID,
                                CaseAssignmentUsers = @case.CASE_ASSIGNMENT_USER.Where(g => g.ASSIGNMENT_ID == x.ASSIGNMENT_ID && g.CASE_ID == x.CASE_ID).Select(c => new CaseAssignmentUser
                                {
                                    ID = c.ID,
                                    NodeID = c.NODE_ID,
                                    NodeName = c.NODE_NAME,
                                    UserName = c.USER_NAME,
                                    CaseComplainedUserType = (CaseComplainedUserType)c.COMPLAINED_USER_TYPE,
                                    OrganizationType = c.ORGANIZATION_TYPE.HasValue ? (OrganizationType)c.ORGANIZATION_TYPE : default(OrganizationType?),
                                    IsApply = c.IS_APPLY
                                }).ToList()

                            }).ToList() ?? new List<CaseAssignment>(),

                        Items = @case.CASE_ITEM.Select(x => new CaseItem
                        {
                            ItemID = x.ITEM_ID,
                            JContent = x.JCONTENT,
                            Item = @case.ITEM.Where(c => c.ID == x.ITEM_ID) == null ? null : new Item<ExpandoObject>
                            {
                                Code = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().CODE,
                                Name = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().NAME,
                                JContent = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().J_CONTENT,
                            }
                        }).ToList(),

                        JContent = @case.J_CONTENT


                    };

                    lock (_lock)
                    {
                        caseList.Add(_case);
                    }

                });
                

            });




            // 回填資料
            Parallel.ForEach(temp, item => 
            {
                var @case = caseList.FirstOrDefault(y => y.CaseID == item.CaseID);

                item.JContent = @case.JContent;
                item.CaseConcatUsersList = @case?.CaseConcatUsers?.ToList() ?? new List<CaseConcatUser>();
                item.CaseComplainedUsersList = @case?.CaseComplainedUsers?.ToList() ?? new List<CaseComplainedUser>();
                item.CaseTagList = @case?.CaseTags;
                item.CaseFinishReasonDatas = @case?.CaseFinishReasonDatas;
                item.CaseAssignmentUsersList = @case.CaseAssignments.Where(y => y.AssignmentID == item.AssignmentID && y.CaseID == item.CaseID).FirstOrDefault().CaseAssignmentUsers;
                //商品
                item.CaseItemList = @case?.Items;
            });
            

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            var data = new List<SP_GetCaseAssignmentHSList>();
            //篩選連絡者 被反應者、轉派對象
            var con = new MSSQLCondition<SP_GetCaseAssignmentHSList>(condition);
            data.AddRange(temp.Query(con));
            //20201208 - IsBusinessAll為true時不須過濾(BU查詢功能)
            if (!condition.IsBusinessAll && (condition.AssignmentUser == null || condition.AssignmentUser.Count <= 0))
                // 過濾轉派不屬於當前 組織樹型態(organizationType)的資料
                data = data.Where(x => x.CaseAssignmentUsersList.Any(g => g.OrganizationType == organizationType)).ToList();
            

            var result = _CaseAssignmentHSIntegrationData[nameof(CaseAssignmentHSData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(總部、門市)SP
        /// 反應單
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentInvoiceForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_COMPLAINT_INVOICE_HEADQUARTER_STORE(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                headNodeID,
                (byte)organizationType
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentHSList>>(list);

            //取得其他必要資料
            var tempData = CaseAssignmentSearchMoreData(temp);
            
            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }

            var data = new List<SP_GetCaseAssignmentHSList>();
            //篩選連絡者 被反應者、反應單號、反應類別
            var con = new MSSQLCondition<SP_GetCaseAssignmentHSList>(condition);
            data.AddRange(tempData.Query(con));

            var result = _CaseAssignmentHSIntegrationData[nameof(CaseAssignmentHSInvoiceData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(總部、門市)SP
        /// 通知單
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentNoticeForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_COMPLAINT_NOTICE_HEADQUARTER_STORE(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                headNodeID,
                (byte)organizationType
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentHSList>>(list);


            //取得其他必要資料
            var tempData = CaseAssignmentSearchMoreData(temp);


            var data = new List<SP_GetCaseAssignmentHSList>();
            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            //篩選連絡者 被反應者
            var con = new MSSQLCondition<SP_GetCaseAssignmentHSList>(condition);
            data.AddRange(tempData.Query(con));
            var result = _CaseAssignmentHSIntegrationData[nameof(CaseAssignmentHSNoticeData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得轉派案件查詢清單(總部、門市)SP
        /// 聯絡紀錄
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExcelCaseAssignmentList> GetCaseAssignmentCommuncateForHSLists(CaseAssignmentHSCondition condition, string headNodeID, OrganizationType organizationType)
        {
            var en = new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn);
            var list = en.SP_ASSIGNMENT_COMPLAINT_COMMUNICATE_HEADQUARTER_STORE(
                condition.NodeID,
                condition.Type,
                condition.CaseID,
                condition.CreateStarTime,
                condition.CreateEndTime,
                condition.NoticeDateStarTime,
                condition.NoticeDateEndTime,
                condition.NoticeContent,
                condition.CaseContent,
                condition.ClassificationIDGroup,
                headNodeID,
                (byte)organizationType
                ).ToList();
            var temp = AutoMapper.Mapper.Map<List<SP_GetCaseAssignmentHSList>>(list);


            //取得其他必要資料
            var tempData = CaseAssignmentSearchMoreData(temp);
            
            var data = new List<SP_GetCaseAssignmentHSList>();

            switch (condition.CaseConcatUnitType)
            {
                case UnitType.Store:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetStoreHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ConcatNode != null && condition.ConcatNode.Any())
                        condition.ConcatNodeRange = GetOrganizationHeadQuartersNode(condition.ConcatNode.LastOrDefault().LeftBoundary, condition.ConcatNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            switch (condition.CaseComplainedUnitType)
            {
                case UnitType.Store:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetStoreHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                case UnitType.Organization:
                    if (condition.ComplainedNode != null && condition.ComplainedNode.Any())
                        condition.ComplainedRange = GetOrganizationHeadQuartersNode(condition.ComplainedNode.LastOrDefault().LeftBoundary, condition.ComplainedNode.LastOrDefault().RightBoundary);
                    break;
                default:
                    break;
            }
            //篩選連絡者 被反應者
            var con = new MSSQLCondition<SP_GetCaseAssignmentHSList>(condition);
            data.AddRange(tempData.Query(con));
            var result = _CaseAssignmentHSIntegrationData[nameof(CaseAssignmentHSCommuncateData)].DataToDomainModel(data, condition);

            return result;

        }

        /// <summary>
        /// 取得門市 Node清單
        /// </summary>
        /// <param name="leftBoundary"></param>
        /// <param name="rightBoundary"></param>
        /// <returns></returns>
        private List<int?> GetStoreHeadQuartersNode(int leftBoundary, int rightBoundary)
        {
            var conConcat = new MSSQLCondition<HEADQUARTERS_NODE>();
            conConcat.And(x => x.LEFT_BOUNDARY >= leftBoundary && x.RIGHT_BOUNDARY <= rightBoundary);
            conConcat.And(x => x.NODE_TYPE_KEY == NodeDefinitionValue.Store);
            return _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conConcat).Select(x => x.NodeID).Cast<int?>().ToList();
        }
        /// <summary>
        /// 取得組織 Node清單
        /// </summary>
        /// <param name="leftBoundary"></param>
        /// <param name="rightBoundary"></param>
        /// <returns></returns>
        private List<int?> GetOrganizationHeadQuartersNode(int leftBoundary, int rightBoundary)
        {
            var conComplaint = new MSSQLCondition<HEADQUARTERS_NODE>();
            conComplaint.And(x => x.LEFT_BOUNDARY >= leftBoundary && x.RIGHT_BOUNDARY <= rightBoundary);
            conComplaint.And(x => x.NODE_TYPE_KEY != NodeDefinitionValue.Store);
            return _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(conComplaint).Select(x => x.NodeID).Cast<int?>().ToList();
        }


        /// <summary>
        /// 取得 總部/門市 未銷轉派
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public PagedList<CaseAssignment> GetHeadquarterUnFinishCaseAssignment(HeaderQuarterSummarySearchType searchType, MSSQLCondition<CASE_ASSIGNMENT> con)
        {

            this.UnFinishSearchRule<HeaderQuarterJobPosition>(searchType, con);

            con.And(x => x.CASE_ASSIGNMENT_USER.Any(g => g.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter));

            return _CaseAggregate.CaseAssignment_T1_T2_.GetPaging(con);
        }

        /// <summary>
        /// 取得 總部/門市 未銷轉派數
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public int GetHeadquarterUnFinishCaseAssignmentCount(HeaderQuarterSummarySearchType searchType)
        {

            var con = new MSSQLCondition<CASE_ASSIGNMENT>();

            this.UnFinishSearchRule<HeaderQuarterJobPosition>(searchType, con);
            
            con.And(x => x.CASE_ASSIGNMENT_USER.Any(g => g.ORGANIZATION_TYPE == (int)OrganizationType.HeaderQuarter));

            return _CaseAggregate.CaseAssignment_T1_T2_.Count(con);
        }

        /// <summary>
        /// 取得 廠商 未銷轉派
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public PagedList<CaseAssignment> GetVenderUnFinishCaseAssignment(HeaderQuarterSummarySearchType searchType, MSSQLCondition<CASE_ASSIGNMENT> con)
        {

            this.UnFinishSearchRule<VendorJobPosition>(searchType, con);

            con.And(x => x.CASE_ASSIGNMENT_USER.Any(g => g.ORGANIZATION_TYPE == (int)OrganizationType.Vendor));

            return _CaseAggregate.CaseAssignment_T1_T2_.GetPaging(con);
        }

        /// <summary>
        /// 取得 廠商 未銷轉派數
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public int GetVenderUnFinishCaseAssignmentCount(HeaderQuarterSummarySearchType searchType)
        {
            var con = new MSSQLCondition<CASE_ASSIGNMENT>();

            this.UnFinishSearchRule<VendorJobPosition>(searchType, con);

            con.And(x => x.CASE_ASSIGNMENT_USER.Any(g => g.ORGANIZATION_TYPE == (int)OrganizationType.Vendor));

            return _CaseAggregate.CaseAssignment_T1_T2_.Count(con);
        }
        /// <summary>
        /// 取得問題分類底下問題分類代號
        /// </summary>
        /// <param name="ClassificationID"></param>
        /// <returns></returns>
        public async Task<string> GetClassificationIDGroup(int ClassificationID)
        {
            string ClassificationIDGroup = "";

            //取得問題分類物件
            var includeStr = "Select".NestedLambdaString("QUESTION_CLASSIFICATION1", "c => c.QUESTION_CLASSIFICATION1", 5);
            var expression = await typeof(QUESTION_CLASSIFICATION).IncludeExpression<Func<QUESTION_CLASSIFICATION, object>>(includeStr);

            var con = new MSSQLCondition<QUESTION_CLASSIFICATION>();
            con.IncludeBy(expression);
            con.And(c => c.ID == ClassificationID);
            //大分類項目
            var qcfList = _MasterAggregate.QuestionClassification_T1_T2_.Get(con);

            if (qcfList != null)
            {
                ClassificationIDGroup = string.Join(",", qcfList.Flatten().Select(x => x.ID));
            }
            return ClassificationIDGroup;
        }

        #region Private Method


        private void UnFinishSearchRule<T>(HeaderQuarterSummarySearchType searchType, MSSQLCondition<CASE_ASSIGNMENT> con) where T : JobPosition, new()
        {

            var user = ContextUtility.GetUserIdentity().Instance;
            var jobPos = user.JobPositions.OfType<T>().Cast<JobPosition>().ToList();

            switch (searchType)
            {
                case HeaderQuarterSummarySearchType.UnFinishCase:
                    jobPos.ForEach(nodeJob =>
                    {
                        con.Or(x => x.CASE_ASSIGNMENT_USER.Any(g => g.NODE_ID == nodeJob.NodeID));
                    });
                    
                    con.And(x => x.CASE_ASSIGNMENT_TYPE == (int)CaseAssignmentType.Assigned);
                    break;
                case HeaderQuarterSummarySearchType.UnFinishRejectCase:

                    var nodeIDs = jobPos.Select(x => x.NodeID).Cast<int?>();

                    jobPos.ForEach(nodeJob =>
                    {
                        con.Or(x => x.CASE_ASSIGNMENT_USER.Any(g => g.NODE_ID == nodeJob.NodeID));
                    });

                    con.And(x =>
                        x.REJECT_TYPE == (int)RejectType.Undo ||
                        (x.REJECT_TYPE == (int)RejectType.FillContent && nodeIDs.Contains(x.FINISH_NODE_ID))
                       );



                    break;
                case HeaderQuarterSummarySearchType.UnFinishUnderCase:

                    // 取得 底下結點(目前只針對 總部/廠商 做運算)
                    var selfUnderNodeIDs = typeof(T) == typeof(HeaderQuarterJobPosition) ?
                                                this.GetHeadquarterSelfUnderNodeIDs() :
                                                this.GetVenderSelfUnderNodeIDs();

                    var selfNodeIDs = jobPos.Select(x => x.NodeID);

                    // 取得差集(排除自己所在的節點)
                    var IntersectionNodeIDs = selfUnderNodeIDs.Except(selfNodeIDs).ToList();

                    if (IntersectionNodeIDs.Any())
                    {
                        IntersectionNodeIDs.ForEach(nodeID =>
                        {
                            con.Or(x => x.CASE_ASSIGNMENT_USER.Any(g => g.NODE_ID == nodeID));
                        });
                    }
                    else
                    {
                        con.And(x => x.CASE_ASSIGNMENT_USER.Any(g => g.NODE_ID == -1));
                    }


                    con.And(x => x.CASE_ASSIGNMENT_TYPE == (int)CaseAssignmentType.Assigned);

                    break;
                default:
                    break;
            }


        }

        private List<int> GetHeadquarterSelfUnderNodeIDs()
        {

            var user = ContextUtility.GetUserIdentity().Instance;
            var jobPos = user.JobPositions.OfType<HeaderQuarterJobPosition>().Cast<JobPosition>().ToList();

            var hqCon = new MSSQLCondition<HEADQUARTERS_NODE>();
            hqCon.FilterNodeFromPosition<HEADQUARTERS_NODE>(jobPos);

            return _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetListOfSpecific(hqCon, x => x.NODE_ID);
        }

        private List<int> GetVenderSelfUnderNodeIDs()
        {

            var user = ContextUtility.GetUserIdentity().Instance;
            var jobPos = user.JobPositions.OfType<VendorJobPosition>().Cast<JobPosition>().ToList();

            var hqCon = new MSSQLCondition<VENDOR_NODE>();
            hqCon.FilterNodeFromPosition<VENDOR_NODE>(jobPos);

            return _OrganizationAggregate.VendorNode_T1_T2_.GetListOfSpecific(hqCon, x => x.NODE_ID);
        }

        /// <summary>
        /// 案件查詢 取得更多資料，同時只取必要資料
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private List<SP_GetCaseList> CaseSearchMoreData(List<SP_GetCaseList> temp)
        {
            var caseIDList = temp.Select(y => y.CaseID);
            var caseList = new List<Domain.Case.Case>();
            var _lock = new object();

            _CaseAggregate.Case_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                db.Configuration.LazyLoadingEnabled = false;

                // 取得 1 => ** 資料
                var query = db.CASE
                                   .AsQueryable()
                                   .AsNoTracking()
                                    .Include(x => x.CASE_CONCAT_USER)
                                    .Include(x => x.CASE_COMPLAINED_USER)
                                    .Include(x => x.CASE_TAG)
                                    .Include(x => x.CASE_FINISH_REASON_DATA)
                                    .Include(x => x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION))
                                    .Include(x => x.CASE_ASSIGNMENT_COMPLAINT_INVOICE);

                // 減少查詢欄位
                var entity = query.Where(x => caseIDList.Contains(x.CASE_ID))
                                   .Select(x => new
                                   {
                                       x.CASE_ID,
                                       x.QUESION_CLASSIFICATION_ID,
                                       x.J_CONTENT,

                                       x.CASE_COMPLAINED_USER,
                                       x.CASE_CONCAT_USER,
                                       x.CASE_TAG,
                                       x.CASE_FINISH_REASON_DATA,
                                       CASE_FINISH_REASON_CLASSIFICATION = x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION),
                                       x.CASE_ASSIGNMENT_COMPLAINT_INVOICE,
                                   }).ToList();

                
                // 自己 Mapping 加快速度
                Parallel.ForEach(entity, @case =>
                {

                    var _case = new Domain.Case.Case
                    {

                        CaseID = @case.CASE_ID,
                        QuestionClassificationID = @case.QUESION_CLASSIFICATION_ID,

                        CaseComplainedUsers = @case.CASE_COMPLAINED_USER.Select(x => new CaseComplainedUser
                        {
                            ID = x.ID,
                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            CaseComplainedUserType = (CaseComplainedUserType)x.COMPLAINED_USER_TYPE,
                            OwnerUserName = x.OWNER_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            SupervisorUserName = x.SUPERVISOR_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            OwnerUserPhone = x.OWNER_USER_PHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                        }).ToList(),

                        CaseConcatUsers = @case.CASE_CONCAT_USER.Select(x => new CaseConcatUser
                        {
                            ID = x.ID,
                            Mobile = x.MOBILE,
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            TelephoneBak = x.TELEPHONE_BAK.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Email = x.EMAIL.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Address = x.ADDRESS.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),

                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            UserName = x.USER_NAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Gender = x.GENDER.HasValue ? (GenderType)x.GENDER : default(GenderType?)
                        }).ToList(),

                        CaseTags = @case.CASE_TAG.Select(g => new CaseTag { ID = g.ID, Name = g.NAME }).ToList(),

                        CaseFinishReasonDatas = @case.CASE_FINISH_REASON_DATA.Select(x => new CaseFinishReasonData
                        {
                            ID = x.ID,
                            Text = x.TEXT,
                            CaseFinishReasonClassification = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID) == null ? null : new CaseFinishReasonClassification
                            {
                                Title = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID).FirstOrDefault().TITLE
                            }
                        }).ToList(),

                        ComplaintInvoice = @case.CASE_ASSIGNMENT_COMPLAINT_INVOICE.Select(x => new CaseAssignmentComplaintInvoice
                        {
                            ID = x.ID,
                            InvoiceID = x.INVOICE_ID
                        }).ToList(),

                        JContent = @case.J_CONTENT
                    };


                    lock (_lock)
                    {
                        caseList.Add(_case);
                    }


                });
                
            });

            _QuestionClassificationResolver.ResolveCollection(caseList);


            // 回填資料
            Parallel.ForEach(temp, item =>
            {
                var @case = caseList.FirstOrDefault(y => y.CaseID == item.CaseID);

                item.JContent = @case.JContent;
                item.CaseConcatUsersList = @case?.CaseConcatUsers?.ToList() ?? new List<CaseConcatUser>();
                item.CaseComplainedUsersList = @case?.CaseComplainedUsers?.ToList() ?? new List<CaseComplainedUser>();
                item.CaseTagList = @case?.CaseTags;
                item.CaseFinishReasonDatas = @case?.CaseFinishReasonDatas;
                item.ComplaintInvoice = @case?.ComplaintInvoice;
                item.ClassificationParentName = @case?.QuestionClassificationParentNamesByArray?.FirstOrDefault();
            });
            
            return temp;
        }

        /// <summary>
        /// 轉派查詢 取得更多資料，同時只取必要資料
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        private List<T> CaseAssignmentSearchMoreData<T>(List<T> temp) where T : SP_GetCaseAssignmentListBase
        {

            var caseIDList = temp.Select(y => y.CaseID);
            var caseList = new List<Domain.Case.Case>();
            var _lock = new object();

            _CaseAggregate.Case_T1_T2_.Operator(context =>
            {
                var db = (SMARTIIEntities)context;

                db.Configuration.LazyLoadingEnabled = false;
                

                // 取得 1 => ** 資料
                var query = db.CASE
                                   .AsQueryable()
                                   .AsNoTracking()
                                    .Include(x => x.CASE_CONCAT_USER)
                                    .Include(x => x.CASE_COMPLAINED_USER)
                                    .Include(x => x.CASE_TAG)
                                    .Include(x => x.CASE_FINISH_REASON_DATA)
                                    .Include(x => x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION))
                                    .Include(x => x.CASE_ITEM)
                                    .Include(x => x.CASE_ITEM.Select(c => c.ITEM))
                                    ;

                // 減少查詢欄位
                var entity = query.Where(x => caseIDList.Contains(x.CASE_ID))
                                   .Select(x => new
                                   {
                                       x.CASE_ID,
                                       x.QUESION_CLASSIFICATION_ID,
                                       x.J_CONTENT,

                                       x.CASE_COMPLAINED_USER,
                                       x.CASE_CONCAT_USER,
                                       x.CASE_TAG,
                                       x.CASE_FINISH_REASON_DATA,
                                       CASE_FINISH_REASON_CLASSIFICATION = x.CASE_FINISH_REASON_DATA.Select(c => c.CASE_FINISH_REASON_CLASSIFICATION),
                                       x.CASE_ITEM,
                                       ITEM = x.CASE_ITEM.Select(c => c.ITEM)
                                   }).ToList();


                // 自己 Mapping 加快速度
                Parallel.ForEach(entity, @case =>
                {

                    var _case = new Domain.Case.Case
                    {

                        CaseID = @case.CASE_ID,
                        QuestionClassificationID = @case.QUESION_CLASSIFICATION_ID,

                        CaseComplainedUsers = @case.CASE_COMPLAINED_USER.Select(x => new CaseComplainedUser
                        {
                            ID = x.ID,
                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            CaseComplainedUserType = (CaseComplainedUserType)x.COMPLAINED_USER_TYPE,
                            OwnerUserName = x.OWNER_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            SupervisorUserName = x.SUPERVISOR_USERNAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            OwnerUserPhone = x.OWNER_USER_PHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                        }).ToList(),

                        CaseConcatUsers = @case.CASE_CONCAT_USER.Select(x => new CaseConcatUser
                        {
                            ID = x.ID,
                            Mobile = x.MOBILE,
                            Telephone = x.TELEPHONE.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            TelephoneBak = x.TELEPHONE_BAK.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Email = x.EMAIL.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Address = x.ADDRESS.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),

                            UnitType = (UnitType)x.UNIT_TYPE,
                            StoreNo = x.STORE_NO,
                            NodeName = x.NODE_NAME,
                            NodeID = x.NODE_ID,
                            ParentPathName = x.PARENT_PATH_NAME,
                            UserName = x.USER_NAME.AesDecryptBase64(SecurityCache.Instance.PersonalInfoSecurityKey),
                            Gender = x.GENDER.HasValue ? (GenderType)x.GENDER : default(GenderType?)
                        }).ToList(),

                        CaseTags = @case.CASE_TAG.Select(g => new CaseTag { ID = g.ID, Name = g.NAME }).ToList(),

                        CaseFinishReasonDatas = @case.CASE_FINISH_REASON_DATA.Select(x => new CaseFinishReasonData
                        {
                            ID = x.ID,
                            Text = x.TEXT,
                            CaseFinishReasonClassification = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID) == null ? null : new CaseFinishReasonClassification
                            {
                                Title = @case.CASE_FINISH_REASON_CLASSIFICATION.Where(c => c.ID == x.CLASSIFICATION_ID).FirstOrDefault().TITLE
                            }
                        }).ToList(),

                        Items = @case.CASE_ITEM.Select(x => new CaseItem
                        {
                            ItemID = x.ITEM_ID,
                            JContent = x.JCONTENT,
                            Item = @case.ITEM.Where(c => c.ID == x.ITEM_ID) == null ? null : new Item<ExpandoObject>
                            {
                                Code = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().CODE,
                                Name = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().NAME,
                                JContent = @case.ITEM.Where(c => c.ID == x.ITEM_ID).FirstOrDefault().J_CONTENT,
                            }
                        }).ToList(),

                        JContent = @case.J_CONTENT


                    };

                    lock (_lock)
                    {
                        caseList.Add(_case);
                    }

                });
                
            });


            // 回填資料
            Parallel.ForEach(temp, item =>
            {
                var @case = caseList.FirstOrDefault(y => y.CaseID == item.CaseID);

                item.JContent = @case.JContent;
                item.CaseConcatUsersList = @case?.CaseConcatUsers?.ToList() ?? new List<CaseConcatUser>();
                item.CaseComplainedUsersList = @case?.CaseComplainedUsers?.ToList() ?? new List<CaseComplainedUser>();
                item.CaseTagList = @case?.CaseTags;
                item.CaseFinishReasonDatas = @case?.CaseFinishReasonDatas;
                //商品，
                item.CaseItemList = @case?.Items;
            });


            return temp.ToList();
        }

        #endregion

    }
}
