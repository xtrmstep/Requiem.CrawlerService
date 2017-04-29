using System;
using System.Collections.Generic;
using System.Linq;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class CrawlerSettingsRepository : ICrawlerSettingsRepository
    {
        private readonly IActivityLogRepository _logger;

        public CrawlerSettingsRepository(IActivityLogRepository logger)
        {
            _logger = logger;
        }

        public HostSetting GetSettings(UrlItem urlItem)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    var hostSettings = ctx.HostSettings.SingleOrDefault(s => s.Host == urlItem.Host);
                    if (hostSettings == null)
                    {
                        hostSettings = ctx.HostSettings.Single(s => s.Host == string.Empty);
                    }
                    _logger.SettingsLoaded(urlItem);
                    return hostSettings;
                }
            }
            catch (Exception err)
            {
                _logger.LogError(urlItem, err);
                throw;
            }
        }

        public void SetSettings(UrlItem urlItem, HostSetting settings)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    var existing = ctx.HostSettings.SingleOrDefault(s => s.Host == urlItem.Host);
                    var newSettings = existing ?? settings;
                    newSettings.Host = urlItem.Host; // ensure the host is correct
                    // update if exists
                    if (existing != null)
                    {
                        newSettings.CrawlDelay = settings.CrawlDelay;
                        newSettings.RobotsTxt = settings.RobotsTxt;
                        newSettings.Disallow = settings.Disallow;
                    }
                    ctx.HostSettings.Add(newSettings);
                    ctx.SaveChanges();
                }

                _logger.SettingsStored(urlItem);
            }
            catch (Exception err)
            {
                _logger.LogError(urlItem, err);
                throw;
            }
        }

        public string GetUserAgent()
        {
            return "FreeskiHD Crawler v1.0 (www.binary-notes.ru/web-crawler-design) / development version";
        }

        public IEnumerable<CrawlRule> GetParsingRules(JobItem job)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    var rules = ctx.CrawlRules.ToList();
                    _logger.RulesTaken(job);
                    return rules;
                }
            }
            catch (Exception err)
            {
                _logger.LogError(job.Url.Url, err);
                throw;
            }
        }
    }
}