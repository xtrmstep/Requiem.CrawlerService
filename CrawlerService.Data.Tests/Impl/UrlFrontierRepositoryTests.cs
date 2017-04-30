using System;
using System.Linq;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class UrlFrontierRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public UrlFrontierRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "UrlFrontierRepository returns the first available URL by date")]
        public void Should_return_first_available_url_by_date()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "UrlFrontierRepository returns the first available URL by text")]
        public void Should_return_first_available_url_by_text()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "UrlFrontierRepository returns URL only once")]
        public void Should_return_url_only_once()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "UrlFrontierRepository can add URL")]
        public void Should_add_url()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "UrlFrontierRepository update URL date")]
        public void Should_update_url_date()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "UrlFrontierRepository adds URL only once ")]
        public void Should_add_url_only_once()
        {
            throw new NotImplementedException();
        }
    }
}