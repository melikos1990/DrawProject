using Autofac;
using Ptc.Data.Condition2.Mssql.DI;

namespace SMARTII.Service
{
    public static class DIBuilder
    {
        private static ContainerBuilder Builder = new ContainerBuilder();

        private static IContainer _Container;

        private static void Initialize()
        {
            Builder.RegisterModule(new MSSQLModule());

            _Container = Builder.Build();
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
