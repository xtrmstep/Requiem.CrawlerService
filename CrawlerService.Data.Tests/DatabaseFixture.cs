using System.Transactions;
using CrawlerService.Data.Impl;

namespace CrawlerService.Data
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            using (var ctx = new CrawlerDbContext())
            {
                CrawlerDbContext.SeedDefaults(ctx);
                ctx.SaveChanges();
            }
        }

        public TransactionScope CreateTransaction()
        {
            var to = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };
            // the lowest isolation for tests to avoid deadlocks
            return new TransactionScope(TransactionScopeOption.RequiresNew, to);
        }

        internal CrawlerDbContext CreateDbContext()
        {
            return new CrawlerDbContext();
        }
    }
}