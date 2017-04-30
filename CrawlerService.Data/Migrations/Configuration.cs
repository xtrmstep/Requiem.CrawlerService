namespace CrawlerService.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CrawlerService.Data.Impl.CrawlerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CrawlerService.Data.Impl.CrawlerDbContext context)
        {
            Impl.CrawlerDbContext.SeedDefaults(context);
            context.SaveChanges();
        }
    }
}
