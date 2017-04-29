using System;
using Xunit;

namespace CrawlerService.Data.Helpers
{
    public class UrlItemBuilderTests
    {
        [Fact(DisplayName = "UrlItemBuilder set Domain, EvaliableFromDate")]
        public void Should_correctly_set_URL_and_EvaliableFromDate()
        {
            const string expectedUrl = "http://sub.host.com?p1=v1";
            var expectedDate = new DateTime(2016, 1, 1);
            var urlItem = UrlItemBuilder.Create(expectedUrl, expectedDate);

            Assert.NotNull(urlItem);
            Assert.Equal(expectedUrl, urlItem.Url);
            Assert.Equal(expectedDate, urlItem.EvaliableFromDate);
        }

        [Fact(DisplayName = "UrlItemBuilder set Host")]
        public void Should_correctly_set_Domain()
        {
            const string url = "http://sub.host.com?p1=v1";
            const string expectedHost = "sub.host.com";
            var urlItem = UrlItemBuilder.Create(url, new DateTime(2016, 1, 1));
            Assert.NotNull(urlItem);
            Assert.Equal(expectedHost, urlItem.Host);
        }
    }
}