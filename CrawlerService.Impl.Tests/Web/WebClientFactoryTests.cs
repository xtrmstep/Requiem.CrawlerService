using System;
using System.Net;
using CrawlerService.Data;
using CrawlerService.Types;
using Moq;
using Xunit;

namespace CrawlerService.Impl.Tests.Web
{
    public class WebClientFactoryTests : IDisposable
    {
        public WebClientFactoryTests()
        {
            ImplementationModule.Init();
        }

        [Fact(DisplayName = "WebClientFactory should add correct UserAgent header")]
        public void Should_create_webClient_with_correct_userAgent_header()
        {
            const string expected = "test user agent";

            var settingsMock = new Mock<ICrawlerSettingsRepository>();
            settingsMock.Setup(m => m.GetUserAgent()).Returns(expected);

            var factory = new WebClientFactory(settingsMock.Object);

            using (var wc = factory.CreateWebClient())
            {
                Assert.Equal(expected, wc.GetHeader(HttpRequestHeader.UserAgent));
            }
        }

        public void Dispose()
        {
            ServiceLocator.Reset();
        }
    }
}