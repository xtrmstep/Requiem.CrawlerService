using System.Net;
using CrawlerService.Data;
using CrawlerService.Types;
using CrawlerService.Web;

namespace CrawlerService.Impl.Web
{
    internal class WebClientFactory : IWebClientFactory
    {
        private readonly ICrawlerSettingsRepository _settings;

        public WebClientFactory(ICrawlerSettingsRepository settings)
        {
            _settings = settings;
        }

        public ICrawlerWebClient CreateWebClient()
        {
            var wc = ServiceLocator.Resolve<ICrawlerWebClient>();
            wc.SetHeader(HttpRequestHeader.UserAgent, _settings.GetUserAgent());
            return wc;
        }
    }
}