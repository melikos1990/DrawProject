using Autofac;
using Autofac.Extras.AggregateService;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Organization;
using SMARTII.Service.Organization.Provider;
using SMARTII.Service.Organization.Resolver;
using SMARTII.Service.Organization.Strategy;

namespace SMARTII.Assist.DI
{
    public class OrganizationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CallCenterNodeProcessProvider>().As<IOrganizationNodeProcessProvider>()
                   .Keyed<IOrganizationNodeProcessProvider>(OrganizationType.CallCenter)
                   .InstancePerDependency();

            builder.RegisterType<HeaderQuarterNodeProcessProvider>()
                .As<IOrganizationNodeProcessProvider>()
                  .Keyed<IOrganizationNodeProcessProvider>(OrganizationType.HeaderQuarter)
                  .As<HeaderQuarterNodeProcessProvider>()
                  .InstancePerDependency();

            builder.RegisterType<VendorNodeProcessProvider>().As<IOrganizationNodeProcessProvider>()
                  .Keyed<IOrganizationNodeProcessProvider>(OrganizationType.Vendor)
                  .InstancePerDependency();

            builder.RegisterType<BUStrategy>().As<IOrganizationProcessStrategy>()
                .Keyed<IOrganizationProcessStrategy>(EssentialCache.NodeDefinitionValue.BusinessUnit)
                .InstancePerDependency();

            builder.RegisterType<CallCenterStrategy>().As<IOrganizationProcessStrategy>()
                .Keyed<IOrganizationProcessStrategy>(EssentialCache.NodeDefinitionValue.CallCenter)
                .InstancePerDependency();

            builder.RegisterType<GroupStrategy>().As<IOrganizationProcessStrategy>()
               .Keyed<IOrganizationProcessStrategy>(EssentialCache.NodeDefinitionValue.Group)
               .InstancePerDependency();

            builder.RegisterType<StoreStrategy>().As<IOrganizationProcessStrategy>()
               .Keyed<IOrganizationProcessStrategy>(EssentialCache.NodeDefinitionValue.Store)
               .InstancePerDependency();

            builder.RegisterType<VendorStrategy>().As<IOrganizationProcessStrategy>()
               .Keyed<IOrganizationProcessStrategy>(EssentialCache.NodeDefinitionValue.VendorGroup)
               .InstancePerDependency(); 

            builder.RegisterType<OrganizationNodeResolver>().InstancePerDependency();
            builder.RegisterType<UserResolver>().InstancePerDependency();

            builder.RegisterGeneric(typeof(ExecutiveOrganizationNodeProvider<>))
                   .As(typeof(IExecutiveOrganizationNodeProvider<>))
                   .InstancePerDependency();


            builder.RegisterAggregateService<IOrganizationAggregate>();
        }
    }
}
