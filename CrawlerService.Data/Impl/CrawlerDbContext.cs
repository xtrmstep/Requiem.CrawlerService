using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    public class CrawlerDbContext : DbContext
    {
        private readonly IDbTransaction _dbTransaction;

        public CrawlerDbContext()
            : base("CrawlerDb")
        {
        }
        /// <summary>
        /// Create DB context with wrapping transaction
        /// </summary>
        /// <param name="isolationLevel"></param>
        public CrawlerDbContext(IsolationLevel isolationLevel) : this()
        {
            _dbTransaction = Database.Connection.BeginTransaction(isolationLevel);
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            _dbTransaction?.Commit();
            return result;
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
                Description = "Link"
            };
            var picRule = new ExtractRule
            {
                Type = Types.DataBlocks.PICTURE,
                RegExpression = "<(img)\b[^>]*>",
                Description = "Picture"
            };
            var videoRule = new ExtractRule
            {
                Type = Types.DataBlocks.VIDEO,
                RegExpression = @"(?<=<iframe[^>]*?)(?:\s*width=[""'](?<width>[^""']+)[""']|\s*height=[""'](?<height>[^'""]+)[""']|\s*src=[""'](?<src>[^'""]+[""']))+[^>]*?>",
                Description = "Video"
            };
            ctx.ExtractRules.AddOrUpdate(r => r.Description, linkRule, picRule, videoRule);

            #endregion
        }
    }
}