using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ptc.Data.Condition2;
using SMARTII.Domain.Notification;
using SMARTII.Service.Cache;
using 排程共通依賴;

namespace 解析官網來信
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


            DIBuilder.SetContainer(container);

            //加入來源項目
            DataStorage.Initailize(container);
        }

        private static void Main(string[] args)
        {

            init();

            var service = DIBuilder.Resolve<IEmailService>();

            service.ReceiveEmail();

        }
    }
}
