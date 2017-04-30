using System;
using System.Data.Entity;
using System.Linq;
using CrawlerService.Common.DateTime;
using CrawlerService.Data.Fixtures;
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
            throw new NotImplementedException();
        }
    }
}