using System;
using System.Data.Entity;
using System.Linq;
using CrawlerService.Data.Models;
using Moq;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class DataRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public DataRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "DataRepository should store data block")]
        public void Should_store_data_block()
        {
            const string expectedUrl = "http://test.com";
            const string expectedDataLink = "link";

            using (_db.CreateTransaction())
            {
                var frontier = new DomainNamesRepository();
                var nextAvailableTime = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc); // already available
                frontier.AddOrUpdateUrl(expectedUrl, nextAvailableTime);
                var urlItem = frontier.GetAvailableUrls(1, DateTime.UtcNow).First(); // should return one item

                var jobs = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var jobItem = jobs.Start(urlItem);

                // add a data block by means of the repository
                var dataRep = new DataBlocksRepository(Mock.Of<IActivityLogRepository>());

                var blockId = dataRep.StoreData(jobItem, DataBlockType.Link, expectedDataLink);

                // try to get the data block from DB directly
                using (var ctx = _db.CreateDbContext())
                {
                    var dataBlock = ctx.DataBlocks.Include(b => b.Url).SingleOrDefault(b => b.Id == blockId);

                    Assert.NotNull(dataBlock);

                    Assert.Equal(expectedDataLink, dataBlock.Data);
                    Assert.Equal(expectedUrl, dataBlock.Url.Url);
                }
            }
        }
    }
}