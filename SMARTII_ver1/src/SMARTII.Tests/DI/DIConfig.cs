using Autofac;
using SMARTII._21Century.DI;
using SMARTII.ASO.DI;
using SMARTII.Assist.DI;
using SMARTII.ColdStone.DI;
using SMARTII.COMMON_BU.DI;
using SMARTII.EShop.DI;
using SMARTII.ICC.DI;
using SMARTII.MisterDonut.DI;
using SMARTII.OpenPoint.DI;
using SMARTII.PPCLIFE.DI;

namespace SMARTII.Tests
{
    public static class DIConfig
    {
        public static IContainer Init()
        {
            // 載入其他依賴注射的設定模組

            EntryBuilder.RegisterModule(new COMMON_BUModule());
            EntryBuilder.RegisterModule(new PPCLIFEModule());
            EntryBuilder.RegisterModule(new ASOModule());
            EntryBuilder.RegisterModule(new EShopModule());
            EntryBuilder.RegisterModule(new _21CenturyModule());
            EntryBuilder.RegisterModule(new ICCModule());
            EntryBuilder.RegisterModule(new MisterDonutModule());
            EntryBuilder.RegisterModule(new ColdStoneModule());
            EntryBuilder.RegisterModule(new OpenPointModule());
            EntryBuilder.RegisterModule(new WebModule());
            EntryBuilder.RegisterModule(new MainModule());

            // 將實例建置為DI容器
            var container = EntryBuilder.Build();

            //// 放入全域層級 (WEB API)
            //GlobalConfiguration.Configuration.DependencyResolver =
            //    new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}
