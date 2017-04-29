using Autofac;
using CrawlerService.Data.Impl;

namespace CrawlerService.Data
{
    public class DependencyConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActivityLogRepository>().As<IActivityLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CrawlerSettingsRepository>().As<ICrawlerSettingsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DataRepository>().As<IDataRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JobRepository>().As<IJobRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UrlFrontierRepository>().As<IUrlFrontierRepository>().InstancePerLifetimeScope();
        }
    }
}