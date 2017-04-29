using System.Net;
using CrawlerService.Data;

namespace CrawlerService.Web.Impl
{
    internal class WebClientFactory : IWebClientFactory
    {
        private readonly ICrawlerWebClient _crawlerWebClient;
        private readonly ICrawlerSettingsRepository _settings;

        public WebClientFactory(ICrawlerSettingsRepository settings, ICrawlerWebClient crawlerWebClient)
        {
            _settings = settings;
            _crawlerWebClient = crawlerWebClient;
        }

        public ICrawlerWebClient CreateWebClient()
        {
            // todo this code should be removed because it does not create instance
            _crawlerWebClient.SetHeader(HttpRequestHeader.UserAgent, _settings.GetUserAgent());
            return _crawlerWebClient;
        }
    }
}