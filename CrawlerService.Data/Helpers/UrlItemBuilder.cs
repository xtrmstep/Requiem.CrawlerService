using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Helpers
{
    public static class UrlItemBuilder
    {
        public static DomainName Create(string url, DateTime nextAvailableTime)
        {
            var uri = new Uri(url);
            var urlItem = new DomainName
            {
                Url = url,
                Host = uri.Host,
                EvaliableFromDate = nextAvailableTime
            };
            return urlItem;
        }
    }
}