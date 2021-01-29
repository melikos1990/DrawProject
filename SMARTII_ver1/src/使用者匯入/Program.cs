
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Master;
using 排程共通依賴;

namespace 使用者匯入
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

            //加入檔案位置
            GlobalizationCache.Instance.LoadAssembly();
        }

        private static void Main(string[] args)
        {
            init();
            var common = DIBuilder.Resolve<ICommonAggregate>();
            var service = DIBuilder.Resolve<IImportUserService>();
            var nowdate = DateTime.Now.Date;
            service.ImportUser();

        }
    }
}
