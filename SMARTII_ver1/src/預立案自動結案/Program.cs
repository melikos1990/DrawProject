using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Ptc.Data.Condition2;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using 排程共通依賴;

namespace 預立案自動結案
{
    class Program
    {
        static void init()
        {
            // 初始化Autofac
            var container = DIConfig.Init();

            // 初始化AutoMapper
            MapperConfig.Init(container);

            // 初始化MSSQL CONDITION
            DataAccessConfiguration.Configure(Condition2Config.Setups);


            DIBuilder.SetContainer(container);
        }

        static void Main(string[] args)
        {
            init();

            var serviceLog = DIBuilder.Resolve<ICommonAggregate>();
            var service = DIBuilder.Resolve<ICaseSourceService>();

            //-----start
            //var now = DateTime.Now;
            //serviceLog.Logger.Info("【預立案自動結案】  準備進行排程 , 時間 : {now.ToString()}。");
            //((Logger)serviceLog.Loggers["Database"]).WithProperty("Data", new
            //                                            {
            //                                                CreateDateTime = now,
            //                                                CreateUserName = "system",
            //                                                FeatureTag = "BATCH-預立案自動結案",
            //                                                FeatureName = "BATCH-PRECASE_OVERDUEDELETE",
            //                                                Content = "START",
            //                                                Operator = (int)AuthenticationType.Admin
            //                                            }).Error(string.Empty);

            service.ClosePreventionCaseSource();

            //now = DateTime.Now;
            //serviceLog.Logger.Info("【預立案自動結案】  排程執行結束 , 時間 : {now.ToString()}。");
            //((Logger)serviceLog.Loggers["Database"]).WithProperty("Data", new
            //                                            {
            //                                                CreateDateTime = now,
            //                                                CreateUserName = "system",
            //                                                FeatureTag = "BATCH-預立案自動結案",
            //                                                FeatureName = "BATCH-PRECASE_OVERDUEDELETE",
            //                                                Content = "END",
            //                                                Operator = (int)AuthenticationType.Admin
            //                                            }).Error(string.Empty);
            //-----end

        }

        public class TestA
        {

        }

        public class TestB
        {

        }
    }
}
