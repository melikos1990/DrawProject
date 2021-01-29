using Autofac;
using Autofac.Extras.AggregateService;
using SMARTII.Domain.System;
using SMARTII.Service.Report.Builder;
using SMARTII.Service.System.Resolver;

namespace SMARTII.Assist.DI
{
    public class SystemModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemParameterResolver>().InstancePerDependency();

            builder.RegisterAggregateService<ISystemAggregate>();


            builder.RegisterType<ExcelBuilder>().InstancePerDependency();
        }
    }
}
