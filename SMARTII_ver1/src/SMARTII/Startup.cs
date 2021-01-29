using Microsoft.Owin;
using Owin;
using Ptc.Data.Condition2;
using SMARTII.App_Start;
using SMARTII.Configuration;
using SMARTII.Service.Cache;

[assembly: OwinStartup(typeof(SMARTII.Startup))]

namespace SMARTII
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 初始化Autofac
            var container = DIConfig.Init();

            // 初始化AutoMapper
            MapperConfig.Init(container);

            // 初始化MSSQL CONDITION
            DataAccessConfiguration.Configure(Condition2Config.Setups);

            // 初始化Signalr
            ConfigureSignalR(app, container);

            // 初始化靜態參數
            DataStorage.Initailize(container);
        }
    }
}
