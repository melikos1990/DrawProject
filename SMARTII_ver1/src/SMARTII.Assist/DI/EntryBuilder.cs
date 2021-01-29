using Autofac;
using Autofac.Core;

namespace SMARTII.Assist.DI
{
    public static class EntryBuilder
    {
        private static ContainerBuilder Builder = new ContainerBuilder();

        public static IContainer Container { get; set; }

        public static void RegisterModule(IModule module)
        {
            Builder.RegisterModule(module);
        }

        public static IContainer Build()
        {
            Container = Builder.Build();

            return Container;
        }
    }
}