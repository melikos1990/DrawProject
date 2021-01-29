using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2;
using SMARTII.Domain.Report;
using 排程共通依賴;

namespace 案件追蹤示警提醒
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
        }

        static void Main(string[] args)
        {

            init();


            var service = DIBuilder.Resolve<IReportService>();

            service.CaseRemindNotification();

        }
    }
}
