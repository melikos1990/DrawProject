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

namespace 統一藥品日報表
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

            // ex:2020/06/16 執行，則抓 2020/06/01 00:00:00 - 2020/06/15 23:59:59
            // 因為統藥有 上個月未結案 的Sheet , 為了迎合前端所以時間已這個月的1號開始

            var today = DateTime.Now;
            var startDateTime = new DateTime(today.Year, today.Month, 1);
            var endDateTime = DateTime.Today.AddTicks(-1);

            service.PPCLIFEBatchSendMail(startDateTime, endDateTime, "日", BatchValue.PPCLIFEGroupDaily);

        }
    }
}
