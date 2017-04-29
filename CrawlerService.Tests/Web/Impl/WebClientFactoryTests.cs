using System.Net;
using CrawlerService.Data;
using Moq;
using Xunit;

namespace CrawlerService.Web.Impl
{
    public class WebClientFactoryTests
    {
        [Fact(DisplayName = "WebClientFactory should add correct UserAgent header")]
        public void Should_create_webClient_with_correct_userAgent_header()
        {
            const string expected = "test user agent";

            var settingsMock = new Mock<ICrawlerSettingsRepository>();
            settingsMock.Setup(m => m.GetUserAgent()).Returns(expected);

            var crawlerWebClient = Mock.Of<ICrawlerWebClient>();
            var factory = new WebClientFactory(settingsMock.Object, crawlerWebClient);

            using (var wc = factory.CreateWebClient())
            {
                Assert.Equal(expected, wc.GetHeader(HttpRequestHeader.UserAgent));
            }
        }
    }
}