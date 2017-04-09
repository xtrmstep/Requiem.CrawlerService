using CrawlerService.Data;
using CrawlerService.Impl.Data;
using CrawlerService.Types;
using CrawlerService.Impl.Types.Dataflow;
using CrawlerService.Impl.Web;
using CrawlerService.Types.Dataflow;
using CrawlerService.Web;

namespace CrawlerService.Impl
{
    public static class ImplementationModule
    {
        public static void Init()
        {
            ServiceLocator.RegisterForScope<ICrawlerDbContext, CrawlerDbContext>();
            ServiceLocator.RegisterForScope<IDataRepository, DataRepository>();
            ServiceLocator.RegisterForScope<IJobRepository, JobRepository>();
            ServiceLocator.RegisterForScope<IUrlFrontierRepository, UrlFrontierRepository>();
            ServiceLocator.RegisterForScope<ICrawlerSettingsRepository, CrawlerSettingsRepository>();
            ServiceLocator.RegisterForScope<IWebClientFactory, WebClientFactory>();
            ServiceLocator.RegisterForScope<IPipeline, PipelineRoutines>();
            ServiceLocator.RegisterForScope<IActivityLogRepository, ActivityLogRepository>();

            ServiceLocator.RegisterForDependency<ICrawlerWebClient, CrawlerWebClient>();
        }
    }
}