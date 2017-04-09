using Autofac;

namespace CrawlerService.Types
{
    public static class ServiceLocator
    {
        private static IContainer _serviceContainer;

        static ServiceLocator()
        {
            var builder = new ContainerBuilder();
            _serviceContainer = builder.Build();
        }

        public static TInterface Resolve<TInterface>()
        {
            return _serviceContainer.Resolve<TInterface>();
        }

        public static void RegisterForScope<TInterface, TImplementator>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TImplementator>().As<TInterface>().InstancePerLifetimeScope();
            builder.Update(_serviceContainer);
        }

        public static void RegisterForDependency<TInterface, TImplementator>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TImplementator>().As<TInterface>();
            builder.Update(_serviceContainer);
        }

        public static void RegisterForScope<TInterface>(TInterface obj) where TInterface : class
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(obj).InstancePerLifetimeScope();
            builder.Update(_serviceContainer);
        }

        public static void RegisterForDependency<TInterface>(TInterface obj) where TInterface: class
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(obj);
            builder.Update(_serviceContainer);
        }

        public static void Reset()
        {
            var builder = new ContainerBuilder();
            _serviceContainer = builder.Build();
        }
    }
}