using Autofac;
using CrawlerService.Data.Impl;

namespace CrawlerService.Data
{
    public class DependencyConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataBlocksRepository>().As<IDataBlocksRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ProcessesRepository>().As<IProcessesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DomainNamesRepository>().As<IDomainNamesRepository>().InstancePerLifetimeScope();
        }
    }
}