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

namespace 統一藥品數據統計月報表
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
            var now = DateTime.Today;
            var startDateTime = now.AddDays(1 - now.Day).AddMonths(-1);
            var endDateTime = now.AddDays(1 - now.Day).AddMilliseconds(-1);

            service.SendPPCLIFEBrandCalcExcel(startDateTime, endDateTime);

        }
    }
}
