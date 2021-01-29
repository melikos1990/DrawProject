using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2;
using SMARTII.Domain.Common;
using SMARTII.Domain.Report;
using SMARTII.Service.Cache;
using 排程共通依賴;
using static SMARTII.Domain.Cache.EssentialCache;

namespace 統一藥品eShop日報表
{
    internal class Program
    {
        private static void init()
        {
            // 初始化Autofac
            var container = DIConfig.Init();

            // 初始化AutoMapper
            MapperConfig.Init(container);

            // 初始化MSSQL CONDITION
            DataAccessConfiguration.Configure(Condition2Config.Setups);

            // 建立 DI 容器
            DIBuilder.SetContainer(container);

            //加入來源項目
            DataStorage.Initailize(container);
        }

        private static void Main(string[] args)
        {
            init();
            var common = DIBuilder.Resolve<ICommonAggregate>();
            var service = DIBuilder.Resolve<IReportService>();
            var nowdate = DateTime.Today.AddDays(-1);
            var startDateTime = nowdate.AddDays(1 - nowdate.Day).AddMilliseconds(-1);
            var endDateTime = nowdate.AddDays(1).AddMilliseconds(-1);
            // 取得附件密碼
            DataStorage.ZipPassWord.TryGetValue(BusinessKeyValue.EShop, out var password);
            service.SendEhopOnCallExcel(startDateTime, endDateTime, password);

        }
    }
}
