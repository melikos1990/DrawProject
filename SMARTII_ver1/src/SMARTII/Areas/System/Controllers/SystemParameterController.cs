using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.System.Models.SystemParameter;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Configuration;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;

namespace SMARTII.Areas.System.Controllers
{
    [Authentication]
    public class SystemParameterController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggAggregate;
        private readonly ISystemAggregate _SystemAggregate;

        public SystemParameterController(ISystemAggregate SystemAggregate,
                                         ICommonAggregate CommonAggAggregate)
        {
            _SystemAggregate = SystemAggregate;
            _CommonAggAggregate = CommonAggAggregate;
        }

        /// <summary>
        /// 取得清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(SystemParameter_lang.SYSTEM_PARAMETER_GETLIST))]
        [ModelValidator(false)]
        public async Task<PagingResponse> GetList(PagingRequest<SystemParameterSearchViewModel> model)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(
                    model.criteria,
                    model.pageIndex,
                    model.pageSize
                    );
                //只顯示可刪除清單
                con.And(x => x.IS_UNDELETABLE == false);

                con.OrderBy(model.sort, model.orderType);

                var list = _SystemAggregate.SystemParameter_T1_T2_.GetPaging(con);

                var ui = list.Select(x => new SystemparameterListViewModel(x));

                return await new PagingResponse<IEnumerable<SystemparameterListViewModel>>(ui)
                {
                    isSuccess = true,
                    totalCount = list.TotalCount
                }.Async();
            }
            catch (Exception ex)
            {
                _CommonAggAggregate.Logger.Error(
                  ex.PrefixDevMessage(SystemParameter_lang.SYSTEM_PARAMETER_GETLIST_FAIL));

                return await new PagingResponse()
                {
                    isSuccess = false,
                    message = ex.Message
                }.Async();
            }
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(SystemParameter_lang.SYSTEM_PARAMETER_GET))]
        [ModelValidator]
        public async Task<IHttpResult> Get([Required]string ID, [Required]string Key)
        {
            try
            {
                var item = _SystemAggregate.SystemParameter_T1_T2_
                                                  .Get(x => x.ID == ID && x.KEY == Key);

                if (item == null)
                    throw new Exception(Common_lang.NOT_FOUND_DATA);

                var result = new JsonResult<SystemParameterDetailViewModel>(
                                   new SystemParameterDetailViewModel(item), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggAggregate.Logger.Error(
                    ex.PrefixDevMessage(SystemParameter_lang.SYSTEM_PARAMETER_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(SystemParameter_lang.SYSTEM_PARAMETER_GET_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Add)]
        [Logger(nameof(SystemParameter_lang.SYSTEM_PARAMETER_CREATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Create(SystemParameterDetailViewModel Model)
        {
            try
            {
                SystemParameter domain = new SystemParameter()
                {
                    CreateDateTime = DateTime.Now,
                    CreateUserName = UserIdentity.Name,
                    ActiveDateTime = !string.IsNullOrWhiteSpace(Model.ActiveDateTime)? DateTime.ParseExact(Model.ActiveDateTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) : (DateTime?)null,
                    NextValue = Model.NextValue,
                    Key = Model.Key,
                    Text = Model.Text,
                    Value = Model.Value,
                    ID = Model.ID
                };

                _SystemAggregate.SystemParameter_T1_T2_.Add(domain);

                return await new JsonResult(
                    SystemParameter_lang.SYSTEM_PARAMETER_CREATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggAggregate.Logger.Error(
                    ex.PrefixDevMessage(SystemParameter_lang.SYSTEM_PARAMETER_CREATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(SystemParameter_lang.SYSTEM_PARAMETER_CREATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(SystemParameter_lang.SYSTEM_PARAMETER_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(SystemParameterDetailViewModel Model)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>(x => x.ID == Model.ID &&
                                                                    x.KEY == Model.Key);
                con.ActionModify(x =>
                {
                    x.UPDATE_DATETIME = DateTime.Now;
                    x.UPDATE_USERNAME = UserIdentity.Name;
                    x.ACTIVE_DATETIME = !string.IsNullOrWhiteSpace(Model.ActiveDateTime) ? DateTime.ParseExact(Model.ActiveDateTime, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture) : (DateTime?)null;
                    x.NEXT_VALUE = Model.NextValue;
                    x.TEXT = Model.Text;
                    x.VALUE = Model.Value;
                });

                _SystemAggregate.SystemParameter_T1_T2_.Update(con);

                // 需異動 static 
                // 當 IS_UNDELETABLE
                DataStorage.Refresh();

                return await new JsonResult(
                    SystemParameter_lang.SYSTEM_PARAMETER_UPDATE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggAggregate.Logger.Error(
                    ex.PrefixDevMessage(SystemParameter_lang.SYSTEM_PARAMETER_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(SystemParameter_lang.SYSTEM_PARAMETER_UPDATE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(SystemParameter_lang.SYSTEM_PARAMETER_DELETE))]
        [ModelValidator]
        public async Task<IHttpResult> Delete([Required]string ID, [Required]string Key)
        {
            try
            {
                var isSuccess = _SystemAggregate.SystemParameter_T1_T2_
                                                       .Remove(x => x.ID == ID && x.KEY == Key);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                    SystemParameter_lang.SYSTEM_PARAMETER_DELETE_SUCCESS, true)
                    .Async();
            }
            catch (Exception ex)
            {
                _CommonAggAggregate.Logger.Error(
                   ex.PrefixDevMessage(SystemParameter_lang.SYSTEM_PARAMETER_DELETE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(SystemParameter_lang.SYSTEM_PARAMETER_DELETE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Delete)]
        [Logger(nameof(SystemParameter_lang.SYSTEM_PARAMETER_DELETE_RANGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteRange(List<SystemParameterDetailViewModel> Model)
        {
            try
            {
                var con = new MSSQLCondition<SYSTEM_PARAMETER>();

                Model.ForEach(g => con.Or(x => x.ID == g.ID && x.KEY == g.Key));

                var isSuccess = _SystemAggregate.SystemParameter_T1_.RemoveRange(con);

                if (isSuccess == false)
                    throw new Exception(Common_lang.DATABASE_DELETE_ERROR);

                return await new JsonResult(
                 SystemParameter_lang.SYSTEM_PARAMETER_DELETE_SUCCESS, true)
                 .Async();
            }
            catch (Exception ex)
            {
                _CommonAggAggregate.Logger.Error(
                    ex.PrefixDevMessage(SystemParameter_lang.SYSTEM_PARAMETER_DELETE_RANGE_FAILED));

                return await new JsonResult(
                    ex.PrefixMessage(SystemParameter_lang.SYSTEM_PARAMETER_DELETE_RANGE_SUCCESS), false)
                    .Async();
            }
        }
    }
}
