using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.System;

namespace SMARTII.Service.System.Service
{
    public class SystemParameterService : ISystemParameterService
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;

        public SystemParameterService(ICommonAggregate CommonAggregate,
                                      ISystemAggregate SystemAggregate)
        {
            _CommonAggregate = CommonAggregate;
            _SystemAggregate = SystemAggregate;
        }

        public void Refresh()
        {
            _CommonAggregate.Logger.Info("【刷新系統參數】  開始排程");

            try
            {
                var startCon = new MSSQLCondition<SYSTEM_PARAMETER>();
                startCon.And(x => x.ACTIVE_DATETIME.HasValue == true && DateTime.Now >= x.ACTIVE_DATETIME);
                var list = _SystemAggregate.SystemParameter_T1_.GetList(startCon);
                var needRefreshApp = false;

                if (list == null || list.Count() == 0)
                {
                    _CommonAggregate.Logger.Info($"【刷新系統參數】  並未撈出資料 , 故不往下執行。");
                    return;
                }

                _CommonAggregate.Logger.Info($"【刷新系統參數】  撈出共 {list.Count()} 筆。");

                var con = new MSSQLCondition<SYSTEM_PARAMETER>();

                foreach (var item in list)
                {
                    try
                    {
                        if (item.ACTIVE_DATETIME.HasValue)
                        {
                            if (DateTime.Now >= item.ACTIVE_DATETIME.Value)
                            {
                                _CommonAggregate.Logger.Info($"【刷新系統參數】  參數ID : {item.ID} , 參數KEY : {item.KEY} , 變更前 : {item.VALUE}, 變更後 : {item.NEXT_VALUE}。");

                                con.And(x => x.ID == item.ID && x.KEY == item.KEY);
                                con.ActionModify(x =>
                                {
                                    x.UPDATE_DATETIME = DateTime.Now;
                                    x.UPDATE_USERNAME = GlobalizationCache.APName;
                                    x.VALUE = x.NEXT_VALUE;
                                    x.NEXT_VALUE = null;
                                    x.ACTIVE_DATETIME = null;
                                });

                                _SystemAggregate.SystemParameter_T1_.Update(con);

                                con.ClearFilters();
                                con.ClearActionModifies();

                                needRefreshApp = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(ex.Message);
                        _CommonAggregate.Loggers["Email"].Error($"【刷新系統參數】失敗 , 參數ID : {item.ID} , 參數KEY : {item.KEY} , 原因 : {ex.Message}");

                        continue;
                    }
                }

                if (needRefreshApp) { this.CallRefreshApi(); }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.Message);
                _CommonAggregate.Loggers["Email"].Error($"【刷新系統參數】整批失敗, 原因 : {ex.Message}");
            }

            _CommonAggregate.Logger.Info("【刷新系統參數】  結束排程");
        }



        public async Task CallRefreshApi()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var url = WebConfigurationManager.AppSettings["REFRESH_APP"];


                    _CommonAggregate.Logger.Info($"【刷新系統參數】開始 呼叫api => {url}");

                    client.Timeout = TimeSpan.FromSeconds(30);
                    HttpResponseMessage response = await client.GetAsync(url);



                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    _CommonAggregate.Logger.Info($"【刷新系統參數】呼叫api => {url} 成功");
                }
                catch (HttpRequestException e)
                {
                    _CommonAggregate.Logger.Error($"【刷新系統參數】呼叫api 失敗");
                    _CommonAggregate.Logger.Error(e);
                }
            }


        }
    }
}
