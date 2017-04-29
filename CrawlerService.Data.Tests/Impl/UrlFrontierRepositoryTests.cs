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
            #region arrange data

            const string url1 = "http://sub.domain.com?p1=v1";
            const string url2 = "http://sub.domain.com?p1=v2";
            var expectedDate1 = new DateTime(2016, 1, 1);
            var expectedDate2 = new DateTime(2016, 1, 2);
            var asOfDate = new DateTime(2016, 1, 3);
            var urlRep = new DomainNamesRepository();

            using (_db.CreateTransaction())
            {
                urlRep.AddOrUpdateUrl(url1, expectedDate1);
                urlRep.AddOrUpdateUrl(url2, expectedDate2);

                #endregion

                var url = urlRep.GetAvailableUrls(1, asOfDate).First();

                Assert.NotNull(url);
                Assert.Equal(url1, url.Url);
            }
        }

        [Fact(DisplayName = "UrlFrontierRepository returns the first available URL by text")]
        public void Should_return_first_available_url_by_text()
        {
            #region arrange data

            const string url1 = "http://sub.domain.com?p1=v1";
            const string url2 = "http://sub.domain.com?p1=v2";
            var expectedDate = new DateTime(2016, 1, 1);
            var asOfDate = new DateTime(2016, 1, 3);
            var urlRep = new DomainNamesRepository();

            using (_db.CreateTransaction())
            {
                urlRep.AddOrUpdateUrl(url2, expectedDate);
                urlRep.AddOrUpdateUrl(url1, expectedDate);

                #endregion

                var url = urlRep.GetAvailableUrls(1, asOfDate).First();

                Assert.NotNull(url);
                Assert.Equal(url1, url.Url);
            }
        }

        [Fact(DisplayName = "UrlFrontierRepository returns URL only once")]
        public void Should_return_url_only_once()
        {
            #region arrange data

            const string url1 = "http://sub.domain.com?p1=v1";
            const string url2 = "http://sub.domain.com?p1=v2";
            var expectedDate1 = new DateTime(2016, 1, 1);
            var expectedDate2 = new DateTime(2016, 1, 2);
            var asOfDate = new DateTime(2016, 1, 3);

            using (_db.CreateTransaction())
            {
                var urlRep = new DomainNamesRepository();
                urlRep.AddOrUpdateUrl(url1, expectedDate1);
                urlRep.AddOrUpdateUrl(url2, expectedDate2);

                #endregion

                urlRep.GetAvailableUrls(1, asOfDate);
                var url = urlRep.GetAvailableUrls(1, asOfDate).First();

                Assert.NotNull(url);
                Assert.Equal(url2, url.Url);
            }
        }

        [Fact(DisplayName = "UrlFrontierRepository can add URL")]
        public void Should_add_url()
        {
            const string url = "http://sub.domain.com?p1=v1&p2=v2";
            const string host = "sub.domain.com";
            var expectedDate = new DateTime(2016, 1, 1);

            var urlRep = new DomainNamesRepository();
            using (_db.CreateTransaction())
            {
                urlRep.AddOrUpdateUrl(url, expectedDate);
                using (var ctx = _db.CreateDbContext())
                {
                    var dbUrlItem = ctx.DomainNames.SingleOrDefault(u => u.Url == url);

                    Assert.NotNull(dbUrlItem);
                    Assert.Equal(url, dbUrlItem.Url);
                    Assert.Equal(host, dbUrlItem.Host);
                }
            }
        }

        [Fact(DisplayName = "UrlFrontierRepository update URL date")]
        public void Should_update_url_date()
        {
            const string url = "http://sub.domain.com?p1=v1&p2=v2";
            var expectedDate1 = new DateTime(2016, 1, 1);
            var expectedDate2 = new DateTime(2016, 1, 2);

            var urlRep = new DomainNamesRepository();
            using (_db.CreateTransaction())
            {
                urlRep.AddOrUpdateUrl(url, expectedDate1);
                urlRep.AddOrUpdateUrl(url, expectedDate2);
                using (var ctx = _db.CreateDbContext())
                {
                    var dbUrlItem = ctx.DomainNames.SingleOrDefault(u => u.Url == url);

                    Assert.NotNull(dbUrlItem);
                    Assert.Equal(url, dbUrlItem.Url);
                    Assert.Equal(expectedDate2, dbUrlItem.EvaliableFromDate);
                }
            }
        }

        [Fact(DisplayName = "UrlFrontierRepository adds URL only once ")]
        public void Should_add_url_only_once()
        {
            const string url = "http://sub.domain.com?p1=v1&p2=v2";
            var expectedDate1 = new DateTime(2016, 1, 1);

            var urlRep = new DomainNamesRepository();
            using (_db.CreateTransaction())
            {
                urlRep.AddOrUpdateUrl(url, expectedDate1);
                urlRep.AddOrUpdateUrl(url, expectedDate1);
                using (var ctx = _db.CreateDbContext())
                {
                    var actualRecords = ctx.DomainNames.Count(u => u.Url == url);
                    Assert.Equal(1, actualRecords);
                }
            }
        }
    }
}