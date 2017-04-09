using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Impl.Data.Helpers
{
    public static class UrlItemBuilder
    {
        public static UrlItem Create(string url, DateTime nextAvailableTime)
        {
            var uri = new Uri(url);
            var urlItem = new UrlItem
            {
                Url = url,
                Host = uri.Host,
                EvaliableFromDate = nextAvailableTime
            };
            return urlItem;
        }
    }
}
