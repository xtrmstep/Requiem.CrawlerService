using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using CrawlerService.Data.Impl;
using CrawlerService.Data.Migrations;

namespace CrawlerService.Data.Fixtures
{
    public class DatabaseFixture
    {
        public DatabaseFixture()
        {
            // make sure DB is created and up-to-date
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CrawlerDbContext, Configuration>());
            using (var ctx = new CrawlerDbContext())
            {
                // make a call to DB in order to migrate it to the latest version
                var r = ctx.ExtractRules.Take(1).ToList();
            }
        }

        internal CrawlerDbContext CreateContext()
        {
            return new TestDbContext();
        }

        private class TestDbContext : CrawlerDbContext, IDisposable
        {
            private readonly TransactionScope _transaction;

            internal TestDbContext()
            {
                // create a transaction scope
                _transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted });
            }

            public new void Dispose()
            {
                _transaction.Dispose();
            }
        }
    }
}