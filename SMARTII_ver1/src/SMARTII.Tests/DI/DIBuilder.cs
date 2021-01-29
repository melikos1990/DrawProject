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
    public static class DIBuilder
    {
        private static IContainer _Container;

        private static void Initialize()
        {
            EntryBuilder.RegisterModule(new COMMON_BUModule());
            EntryBuilder.RegisterModule(new PPCLIFEModule());
            EntryBuilder.RegisterModule(new ASOModule());
            EntryBuilder.RegisterModule(new EShopModule());
            EntryBuilder.RegisterModule(new _21CenturyModule());
            EntryBuilder.RegisterModule(new ICCModule());
            EntryBuilder.RegisterModule(new MisterDonutModule());
            EntryBuilder.RegisterModule(new ColdStoneModule());
            EntryBuilder.RegisterModule(new OpenPointModule());
            EntryBuilder.RegisterModule(new MainModule());
            EntryBuilder.RegisterModule(new WebModule());

            _Container = EntryBuilder.Build();
        }

        public static void SetContainer(IContainer container)
        {
            _Container = container;
        }

        public static T Resolve<T>()
        {
            if (_Container == null)
                Initialize();

            return _Container.Resolve<T>();
        }
    }
}
