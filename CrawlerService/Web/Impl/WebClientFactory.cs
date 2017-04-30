using System.Net;
using CrawlerService.Data;

namespace CrawlerService.Web.Impl
{
    internal class WebClientFactory : IWebClientFactory
    {
        private readonly ICrawlerWebClient _crawlerWebClient;

        public WebClientFactory(ICrawlerWebClient crawlerWebClient)
        {
            _crawlerWebClient = crawlerWebClient;
        }

        public ICrawlerWebClient CreateWebClient()
        {
            // todo this code should be removed because it does not create instance
            _crawlerWebClient.SetHeader(HttpRequestHeader.UserAgent, "");
            return _crawlerWebClient;
        }
    }
}