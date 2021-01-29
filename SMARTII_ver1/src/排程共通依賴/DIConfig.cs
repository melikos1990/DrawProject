
using Autofac;
using SMARTII._21Century.DI;
using SMARTII.ASO.DI;
using SMARTII.Assist.DI;
using SMARTII.ColdStone.DI;
using SMARTII.COMMON_BU.DI;
using SMARTII.Configuration.DI;
using SMARTII.EShop.DI;
using SMARTII.ICC.DI;
using SMARTII.MisterDonut.DI;
using SMARTII.OpenPoint.DI;
using SMARTII.PPCLIFE.DI;
using SMARTII.FORTUNE.DI;

namespace 排程共通依賴
{
    public static class DIConfig
    {
        public static IContainer Init()
        {
            // 載入其他依賴注射的設定模組

            EntryBuilder.RegisterModule(new COMMON_BUModule());
            EntryBuilder.RegisterModule(new PPCLIFEModule());
            EntryBuilder.RegisterModule(new ColdStoneModule());
            EntryBuilder.RegisterModule(new MainModule());
            EntryBuilder.RegisterModule(new ASOModule());
            EntryBuilder.RegisterModule(new FORTUNEModule());
            EntryBuilder.RegisterModule(new MisterDonutModule());
            EntryBuilder.RegisterModule(new _21CenturyModule());
            EntryBuilder.RegisterModule(new EShopModule());
            EntryBuilder.RegisterModule(new OpenPointModule());
            EntryBuilder.RegisterModule(new ICCModule());
            EntryBuilder.RegisterModule(new WebModule());

            // 將實例建置為DI容器
            var container = EntryBuilder.Build();

        
            return container;
        }
    }
}
