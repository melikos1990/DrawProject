using Ptc.Data.Condition2;
using SMARTII.Domain.Case;
using SMARTII.Service.Cache;
using 排程共通依賴;

namespace 逾時未結案通知
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
            var service = DIBuilder.Resolve<ICaseService>();

            service.CaseTimeoutNotice();
        }
    }
}
