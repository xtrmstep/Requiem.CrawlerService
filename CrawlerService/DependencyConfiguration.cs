using Autofac;
using CrawlerService.Types.Dataflow;
using CrawlerService.Types.Dataflow.Impl;
using CrawlerService.Web;
using CrawlerService.Web.Impl;

namespace CrawlerService
{
    public class DependencyConfiguration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PipelineRoutines>().As<IPipeline>().InstancePerLifetimeScope();
            builder.RegisterType<CrawlerWebClient>().As<ICrawlerWebClient>().InstancePerLifetimeScope();
            builder.RegisterType<WebClientFactory>().As<IWebClientFactory>().InstancePerLifetimeScope();
        }
    }
}