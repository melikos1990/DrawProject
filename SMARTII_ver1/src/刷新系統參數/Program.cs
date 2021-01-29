using System;
using System.Linq;
using NLog;
using Ptc.Data.Condition2;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.System;
using 排程共通依賴;

namespace 刷新系統參數
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

            var serviceLog = DIBuilder.Resolve<ICommonAggregate>();
            var service = DIBuilder.Resolve<ISystemParameterService>();
            ////-----start
            //var now = DateTime.Now;

            //serviceLog.Logger.Info("【刷新系統參數】  準備進行排程 , 時間 : {now.ToString()}。");
            //((Logger)serviceLog.Loggers["Database"]).WithProperty("Data", new
            //{
            //    CreateDateTime = now,
            //    CreateUserName = "system",
            //    FeatureTag = "BATCH-刷新系統參數",
            //    FeatureName = "BATCH-REFRESH_SYSTEM_PARAMETER",
            //    Content = "START",
            //    Operator = (int)AuthenticationType.Admin
            //}).Error(string.Empty);

            service.Refresh();

            //now = DateTime.Now;
            //serviceLog.Logger.Info("【刷新系統參數】  排程執行結束 , 時間 : {now.ToString()}。");
            //((Logger)serviceLog.Loggers["Database"]).WithProperty("Data", new
            //{
            //    CreateDateTime = now,
            //    CreateUserName = "system",
            //    FeatureTag = "BATCH-刷新系統參數",
            //    FeatureName = "BATCH-REFRESH_SYSTEM_PARAMETER",
            //    Content = "END",
            //    Operator = (int)AuthenticationType.Admin
            //}).Error(string.Empty);
            ////-----end
        }
    }
}
