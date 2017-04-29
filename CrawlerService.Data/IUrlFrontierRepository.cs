using System;
using System.Collections.Generic;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    /// <summary>
    ///     Provides routines to manage URL frontier
    /// </summary>
    public interface IUrlFrontierRepository
    {
        /// <summary>
        ///     Get next available URL
        /// </summary>
        /// <param name="number"></param>
        /// <param name="asOfDate"></param>
        /// <remarks>
        ///     When URL is added to the frontier date/time is added also which tells when it can be requested next time.
        ///     The information about that can be retrieved from robots.txt or from default settings.
        /// </remarks>
        /// <returns></returns>
        IEnumerable<UrlItem> GetAvailableUrls(int number, DateTime asOfDate);

        /// <summary>
        ///     Add URL to the frontier
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nextAvailableTime">Next date/time the URL can be processed</param>
        void AddOrUpdateUrl(string url, DateTime nextAvailableTime);
    }
}