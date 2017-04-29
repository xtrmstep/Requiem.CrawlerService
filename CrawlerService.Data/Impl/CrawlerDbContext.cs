using System.Data.Entity;
using System.Data.Entity.Migrations;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class CrawlerDbContext : DbContext
    {
        public CrawlerDbContext()
            : base("CrawlerDb")
        {
        }

        public IDbSet<UrlItem> UrlItems { get; set; }
        public IDbSet<HostSetting> HostSettings { get; set; }
        public IDbSet<DataBlock> DataBlocks { get; set; }
        public IDbSet<CrawlRule> CrawlRules { get; set; }
        public IDbSet<JobItem> JobItems { get; set; }
        public IDbSet<ActivityMessage> ActivityMessages { get; set; }

        /// <summary>
        ///     The method inserts to the DB initial values and must be called from Seed() method of the Configuration class
        /// </summary>
        /// <param name="ctx"></param>
        internal static void SeedDefaults(CrawlerDbContext ctx)
        {
            #region rules

            var linkRule = new CrawlRule
            {
                DataType = DataBlockType.Link,
                RegExpression = "(<a.*?>.*?</a>)",
                Name = "Link"
            };
            var picRule = new CrawlRule
            {
                DataType = DataBlockType.Picture,
                RegExpression = "<(img)\b[^>]*>",
                Name = "Picture"
            };
            var videoRule = new CrawlRule
            {
                DataType = DataBlockType.Video,
                RegExpression = @"(?<=<iframe[^>]*?)(?:\s*width=[""'](?<width>[^""']+)[""']|\s*height=[""'](?<height>[^'""]+)[""']|\s*src=[""'](?<src>[^'""]+[""']))+[^>]*?>",
                Name = "Video"
            };
            ctx.CrawlRules.AddOrUpdate(r => r.Name, linkRule, picRule, videoRule);

            #endregion

            #region settings

            var defaultSettings = new HostSetting
            {
                Host = string.Empty,
                CrawlDelay = 60,
                Disallow = string.Empty,
                RobotsTxt = string.Empty
            };
            ctx.HostSettings.AddOrUpdate(s => s.Host, defaultSettings);

            #endregion

            #region urls

            var defaultUrl = new UrlItem
            {
                Url = "http://binary-notes.ru",
                Host = "binary-notes.ru"
            };
            //ctx.UrlItems.AddOrUpdate(s => s.Url, defaultUrl);

            #endregion
        }
    }
}