using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Areas.Master.Models.UserParameter;
using SMARTII.Assist.Authentication;
using SMARTII.Assist.Logger;
using SMARTII.Assist.Web;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Areas.Master.Controllers
{
    [Authentication]
    public class UserParameterController : BaseApiController
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IMasterAggregate _MasterAggregate;

        public UserParameterController(
            ICommonAggregate CommonAggregate,
            IMasterAggregate MasterAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthenticationMethod(AuthenticationType.Read)]
        [Logger(nameof(UserParameter_lang.USER_PARAMETER_GET))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Get([Required]string userID)
        {
            try
            {
                var con = new MSSQLCondition<USER_PARAMETER>(x => x.USER_ID == userID);

                var data = _MasterAggregate.UserParameter_T1_T2_.Get(con);

                var result = new JsonResult<UserParameterDetailViewModel>(
                                   new UserParameterDetailViewModel(data), true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(UserParameter_lang.USER_PARAMETER_GET_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(UserParameter_lang.USER_PARAMETER_GET_FAIL), false)
                    .Async();
            }
        }
        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthenticationMethod(AuthenticationType.Update)]
        [Logger(nameof(UserParameter_lang.USER_PARAMETER_UPDATE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> Update(UserParameterDetailViewModel model)
        {
            try
            {
                var data = model.ToDomain();

                int order = 1;
                data.FavoriteFeature?.ToList().ForEach(x => x.Order = order++);

                var con = new MSSQLCondition<USER_PARAMETER>(x => x.USER_ID == data.UserID);

                var sysParameter = _MasterAggregate.UserParameter_T1_T2_.Get(con);

                con.ActionModify(x =>
                {
                    x.NAVIGATE_OF_NEWBIE = data.NavigateOfNewbie;
                    x.FAVORITE_FEATURE = JsonConvert.SerializeObject(data.FavoriteFeature);

                    if (data.Picture != null)
                        x.IMAGE_PATH = data.ImagePath;
                    else if (data.ImagePath != null && sysParameter.ImagePath != null)
                    {
                        x.IMAGE_PATH = data.ImagePath;
                    }
                    else {
                        x.IMAGE_PATH = null;
                    }
                });

                new FileProcessInvoker((context) =>
                {
                    if (data.Picture != null)
                        FileSaverUtility.SaveUserParameterFile(context, data);

                    _MasterAggregate.UserParameter_T1_T2_.Update(con);

                });



                var result = new JsonResult(UserParameter_lang.USER_PARAMETER_UPDATE_SUCCESS, true);

                return await result.Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                    ex.PrefixDevMessage(UserParameter_lang.USER_PARAMETER_UPDATE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(UserParameter_lang.USER_PARAMETER_UPDATE_FAIL), false)
                    .Async();
            }
        }
    }
}
