using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CrawlerService.Data;
using CrawlerService.Impl.Data;

namespace CrawlerService.Core.Tests
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

        public ICrawlerDbContext CreateDbContext()
        {
            return new CrawlerDbContext();
        }
    }
}
