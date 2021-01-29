using Autofac;
using Autofac.Extras.AggregateService;
using SMARTII.Domain.Master;
using SMARTII.Service.Master.Resolver;
using SMARTII.Service.Master.Service;

namespace SMARTII.Assist.DI
{
    public class MasterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<IMasterAggregate>();

            builder.RegisterType<ItemResolver>().InstancePerDependency();
            builder.RegisterType<QuestionClassificationResolver>().InstancePerDependency();
            builder.RegisterType<OfficeEmailGroupResolver>().InstancePerDependency();
            builder.RegisterType<CaseRemindResolver>().InstancePerDependency();
            builder.RegisterType<StoreResolver>().InstancePerDependency();
            builder.RegisterType<CaseTemplateService>().As<ICaseTemplateService>().InstancePerDependency();

        }
    }
}
