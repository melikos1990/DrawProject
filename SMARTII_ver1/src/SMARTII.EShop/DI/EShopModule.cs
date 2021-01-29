using Autofac;
using Autofac.Integration.WebApi;
using SMARTII.Domain.Report;
using SMARTII.EShop.Service;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.EShop.DI
{
    public class EShopModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(currentAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<EShopFactory>()
                   .As<IEShopFactory>()
                   .InstancePerDependency();

            builder.RegisterType<EShopFacade>()
                   .InstancePerDependency();

            builder.RegisterType<ReportProvider>()
                   .InstancePerDependency();

            builder.RegisterType<COMMON_BU.Service.ReportFacade>()
                   .InstancePerDependency();


            builder.RegisterType<EShopFactory>().As<IImportStoreFactory>()
                  .Keyed<IImportStoreFactory>(BusinessKeyValue.EShop)
                  .InstancePerDependency();

            builder.RegisterApiControllers(currentAssembly);



        }
    }
}
