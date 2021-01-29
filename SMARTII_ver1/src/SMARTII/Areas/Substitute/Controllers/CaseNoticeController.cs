using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Substitute.Models.CaseApply;
using SMARTII.Areas.Substitute.Models.CaseNotice;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Substitute;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Case.Resolver;
using SMARTII.Service.Organization.Resolver;

namespace SMARTII.Areas.Substitute.Controllers
{
    [Authentication]
    public class CaseNoticeController : CaseBaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ICaseAggregate _CaseAggregate;
        private readonly ICaseApplyFacade _CaseApplyFacade;
        private readonly IUserFacade _UserFacade;
        private readonly OrganizationNodeResolver _OrganizationResolver;

        public CaseNoticeController(ICommonAggregate CommonAggAggregate,
                                   ICaseAggregate CaseAggregate,
                                   ICaseApplyFacade CaseApplyFacade,
                                   IUserFacade UserFacade,
                                   OrganizationNodeResolver OrganizationResolver)
        {
            _CommonAggregate = CommonAggAggregate;
            _CaseAggregate = CaseAggregate;
            _CaseApplyFacade = CaseApplyFacade;
            _UserFacade = UserFacade;
            _OrganizationResolver = OrganizationResolver;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(CaseNotice_lang.CASE_NOTICE_GETLIST))]
        [ModelValidator(false)]
        public async Task<IHttpResult> GetList(CaseNoticeSearchViewModel model)
        {
            try
            {
                var userID = ContextUtility.GetUserIdentity()?.Instance.UserID;
                var con = new MSSQLCondition<CASE_NOTICE>();
                con.And(x => x.APPLY_USER_ID == userID);

                if (model.CaseNoticeType != null)
                    con.And(x => x.CASE_NOTICE_TYPE == (byte)model.CaseNoticeType);

                var list = _CaseAggregate.CaseNotice_T1_T2_.GetPaging(con);

                var caseNotice = _CaseResolver.ResolveCollection(list, iCon =>
                {
                    iCon.IncludeBy(x => x.CASE_COMPLAINED_USER);
                    iCon.IncludeBy(x => x.CASE_CONCAT_USER);
                    iCon.IncludeBy(x => x.CASE_SOURCE);
                    iCon.IncludeBy(x => x.CASE_SOURCE.CASE_SOURCE_USER);
                    iCon.IncludeBy(x => x.CASE_WARNING);
                });

                caseNotice.ToList().ForEach(x =>
                {
                    x.NodeID = x.@case.NodeID;
                });

                var ui = _OrganizationResolver.ResolveCollection(caseNotice).Select(x => new CaseNoticeListViewModel(x));

                var result = new JsonResult<IEnumerable<CaseNoticeListViewModel>>(ui, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                  ex.PrefixDevMessage(CaseNotice_lang.CASE_NOTICE_GETLIST_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseNotice_lang.CASE_NOTICE_GETLIST_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 通知確認
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseNotice_lang.CASE_NOTICE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Notice([Required]int? ID)
        {
            try
            {
                var con = new MSSQLCondition<CASE_NOTICE>(x => x.ID == ID);

                _CaseAggregate.CaseNotice_T1_T2_.Remove(con);

                return await new JsonResult(
                    CaseNotice_lang.CASE_NOTICE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseNotice_lang.CASE_NOTICE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseNotice_lang.CASE_NOTICE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次確認
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(CaseNotice_lang.CASE_NOTICE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> NoticeRange(int[] IDs)
        {
            try
            {
                var con = new MSSQLCondition<CASE_NOTICE>(x => IDs.Contains(x.ID));

                _CaseAggregate.CaseNotice_T1_T2_.RemoveRange(con);

                return await new JsonResult(
                    CaseNotice_lang.CASE_NOTICE_RANGE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(CaseNotice_lang.CASE_NOTICE_RANGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(CaseNotice_lang.CASE_NOTICE_RANGE_FAIL), false)
                    .Async();
            }
        }
    }
}
