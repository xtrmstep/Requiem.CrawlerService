using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CrawlerService.Data.Helpers;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class UrlFrontierRepository : IUrlFrontierRepository
    {
        public IEnumerable<UrlItem> GetAvailableUrls(int number, DateTime asOfDate)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var urls = ctx.UrlItems
                    // note: diff in microseconds causes overflow in DB, that's why diff in seconds is used
                    .Where(url => !url.IsInProgress && (url.EvaliableFromDate.HasValue == false || DbFunctions.DiffSeconds(url.EvaliableFromDate, asOfDate) > 0))
                    .OrderBy(url => url.EvaliableFromDate)
                    .ThenBy(url => url.Url)
                    .Take(number)
                    .ToList();
                // mark them as taken to be processed
                foreach (var url in urls)
                {
                    url.IsInProgress = true;
                }
                ctx.SaveChanges();
                return urls;
            }
        }

        public void AddOrUpdateUrl(string url, DateTime nextAvailableTime)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var urlItem = UrlItemBuilder.Create(url, nextAvailableTime);
                var existingUrl = ctx.UrlItems.SingleOrDefault(u => u.Host == urlItem.Host && u.Url == url);
                if (existingUrl != null)
                {
                    existingUrl.EvaliableFromDate = nextAvailableTime;
                    existingUrl.IsInProgress = false; // reset the flag since the url is not processed anymore and available for the next round
                }
                else
                {
                    ctx.UrlItems.Add(urlItem);
                }
                ctx.SaveChanges();
            }
        }
    }
}