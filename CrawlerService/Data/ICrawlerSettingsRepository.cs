using System.Collections.Generic;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    /// <summary>
    /// Provides the crawler's settings
    /// </summary>
    public interface ICrawlerSettingsRepository
    {
        /// <summary>
        /// Returns settings for URL host
        /// </summary>
        /// <param name="urlItem"></param>
        /// <remarks>The host settings define delay and other constraints for each host.
        /// They can be retrieved from robots.txt or from default settings for the crawler.</remarks>
        /// <returns></returns>
        HostSetting GetSettings(UrlItem urlItem);

        /// <summary>
        /// Set settings for URL host
        /// </summary>
        /// <param name="urlItem"></param>
        /// <param name="settings">The updated settings from robots.txt</param>
        /// <returns></returns>
        void SetSettings(UrlItem urlItem, HostSetting settings);

        /// <summary>
        /// Returns the crawler identity name which all web requests should be accompanied with
        /// </summary>
        /// <returns></returns>
        string GetUserAgent();

        /// <summary>
        /// Returns all rules for parsing data blocks
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        IEnumerable<CrawlRule> GetParsingRules(JobItem job);
    }
}