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

        public IDbSet<DomainName> DomainNames { get; set; }
        public IDbSet<DataBlock> DataBlocks { get; set; }
        public IDbSet<ExtractRule> ExtractRules { get; set; }
        public IDbSet<Process> Processes { get; set; }

        /// <summary>
        ///     The method inserts to the DB initial values and must be called from Seed() method of the Configuration class
        /// </summary>
        /// <param name="ctx"></param>
        internal static void SeedDefaults(CrawlerDbContext ctx)
        {
            #region rules

            var linkRule = new ExtractRule
            {
                Type = Types.DataBlocks.LINK,
                RegExpression = "(<a.*?>.*?</a>)",
                Name = "Link"
            };
            var picRule = new ExtractRule
            {
                Type = Types.DataBlocks.PICTURE,
                RegExpression = "<(img)\b[^>]*>",
                Name = "Picture"
            };
            var videoRule = new ExtractRule
            {
                Type = Types.DataBlocks.VIDEO,
                RegExpression = @"(?<=<iframe[^>]*?)(?:\s*width=[""'](?<width>[^""']+)[""']|\s*height=[""'](?<height>[^'""]+)[""']|\s*src=[""'](?<src>[^'""]+[""']))+[^>]*?>",
                Name = "Video"
            };
            ctx.ExtractRules.AddOrUpdate(r => r.Name, linkRule, picRule, videoRule);

            #endregion

            #region urls

            var defaultUrl = new DomainName
            {
                Name = "http://binary-notes.ru"
            };
            ctx.DomainNames.AddOrUpdate(s => s.Name, defaultUrl);

            #endregion
        }
    }
}