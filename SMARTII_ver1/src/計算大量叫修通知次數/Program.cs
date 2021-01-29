using Autofac.Features.Indexed;
using Ptc.Data.Condition2;
using SMARTII.Domain.Common;
using SMARTII.Domain.Notification;
using 排程共通依賴;

namespace 計算大量叫修通知次數
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

        private static void Main(string[] args)
        {
            init();
            var common = DIBuilder.Resolve<ICommonAggregate>();
            var service = DIBuilder.Resolve<INotificationGroupService>();
           // common.Logger.Info("【提醒群組通知】  準備進行排程");
            service.Calculate();
           // common.Logger.Error("【提醒群組通知】  排程執行結束");

        }
    }
}
