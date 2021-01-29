using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
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

namespace SMARTII.Configuration
{
    public static class DIConfig
    {

        private static IContainer _Container { get; set; }

        public static IContainer Init()
        {

            if (_Container != null)
                return _Container;

            // 載入其他依賴注射的設定模組
            LoadAssemblyModules();

            EntryBuilder.RegisterModule(new MainModule());
            EntryBuilder.RegisterModule(new WebModule());

            // 將實例建置為DI容器
            _Container = EntryBuilder.Build();

            // 放入全域層級 (WEB API)
            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(_Container);

            return _Container;
        }

        /// <summary>
        /// 載入模組(OTHER BU)
        /// </summary>
        public static void LoadAssemblyModules()
        {
            EntryBuilder.RegisterModule(new COMMON_BUModule());
            EntryBuilder.RegisterModule(new PPCLIFEModule());
            EntryBuilder.RegisterModule(new ASOModule());
            EntryBuilder.RegisterModule(new EShopModule());
            EntryBuilder.RegisterModule(new _21CenturyModule());
            EntryBuilder.RegisterModule(new MisterDonutModule());
            EntryBuilder.RegisterModule(new ColdStoneModule());
            EntryBuilder.RegisterModule(new ICCModule());
            EntryBuilder.RegisterModule(new OpenPointModule());
            EntryBuilder.RegisterModule(new FORTUNEModule());
        }

        ///// <summary>
        ///// 載入模組(OTHER BU)
        ///// </summary>
        //public static void LoadAssemblyModules()
        //{
        //    GlobalizationCache.Instance.AssemblyDict.ForEach(assem =>
        //    {
        //        var moduleTypes = assem.Value.GetTypes().Where(t => typeof(Autofac.Module).IsAssignableFrom(t));

        //        foreach (var type in moduleTypes)
        //        {
        //            EntryBuilder.RegisterModule((Autofac.Module)Activator.CreateInstance(type));

        //        }

        //        AppDomain.CurrentDomain.Load(assem.Value.GetName());

        //    });
        //}
    }
}
