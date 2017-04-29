using System.Linq;
using CrawlerService.Data.Models;
using Moq;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class CrawlerSettingsRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public CrawlerSettingsRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "Crawler Settings returns UserAgent")]
        public void Should_return_correct_userAgent()
        {
            var settings = new CrawlerSettingsRepository(Mock.Of<IActivityLogRepository>());

            Assert.Equal("FreeskiHD Crawler v1.0 (www.binary-notes.ru/web-crawler-design) / development version", settings.GetUserAgent());
        }

        [Fact(DisplayName = "Crawler Settings returns Host settings")]
        public void Should_return_settings_for_host()
        {
            using (_db.CreateTransaction())
            {
                const string testUrl = "http://sub.testhost.com/page?param=1&param=2";
                const string testHost = "testhost.com";

                #region add test settings for some host

                var testSettings = new HostSetting
                {
                    CrawlDelay = 60,
                    Disallow = null,
                    Host = testHost,
                    RobotsTxt = string.Empty
                };

                using (var ctx = _db.CreateDbContext())
                {
                    ctx.HostSettings.Add(testSettings);
                    ctx.SaveChanges();
                }

                #endregion

                #region get settings for host

                var urlItem = new DomainName
                {
                    Url = testUrl, Host = testHost
                };
                var settingsRep = new CrawlerSettingsRepository(Mock.Of<IActivityLogRepository>());
                var hostSettings = settingsRep.GetSettings(urlItem);

                #endregion

                Assert.NotNull(hostSettings);
                Assert.Equal(testSettings.Host, hostSettings.Host);
                Assert.Equal(testSettings.CrawlDelay, hostSettings.CrawlDelay);
                Assert.Equal(testSettings.RobotsTxt, hostSettings.RobotsTxt);
                Assert.Equal(testSettings.Disallow, hostSettings.Disallow);
            }
        }

        [Fact(DisplayName = "Crawler Settings returns default settings for Host")]
        public void Should_return_default_settings_when_host_dont_have()
        {
            #region get default settings

            HostSetting defaultSettings;
            using (var ctx = _db.CreateDbContext())
            {
                defaultSettings = ctx.HostSettings.Single(s => s.Host == string.Empty);
            }

            #endregion

            const string testUrl = "http://sub.testhost.com/page?param=1&param=2";
            const string testHost = "testhost.com";

            #region get settings for host

            var urlItem = new DomainName
            {
                Url = testUrl, Host = testHost
            };
            var settingsRep = new CrawlerSettingsRepository(Mock.Of<IActivityLogRepository>());
            var hostSettings = settingsRep.GetSettings(urlItem);

            #endregion

            Assert.NotNull(hostSettings);
            Assert.Equal(defaultSettings.Host, hostSettings.Host);
            Assert.Equal(defaultSettings.CrawlDelay, hostSettings.CrawlDelay);
            Assert.Equal(defaultSettings.RobotsTxt, hostSettings.RobotsTxt);
            Assert.True(defaultSettings.Disallow.SequenceEqual(hostSettings.Disallow));
        }

        [Fact(DisplayName = "Crawler Settings stores settings for Host")]
        public void Should_store_host_settings()
        {
            using (_db.CreateTransaction())
            {
                const string testUrl = "http://sub.testhost.com/page?param=1&param=2";
                const string testHost = "testhost.com";

                var testSetting = new HostSetting
                {
                    CrawlDelay = 60,
                    Disallow = null,
                    Host = testHost,
                    RobotsTxt = string.Empty
                };

                #region get settings for host

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };
                var settingsRep = new CrawlerSettingsRepository(Mock.Of<IActivityLogRepository>());
                settingsRep.SetSettings(urlItem, testSetting);

                #endregion

                using (var ctx = _db.CreateDbContext())
                {
                    var storedSetting = ctx.HostSettings.Single(s => s.Host == testHost);

                    Assert.Equal(testSetting.Host, storedSetting.Host);
                    Assert.Equal(testSetting.CrawlDelay, storedSetting.CrawlDelay);
                    Assert.Equal(testSetting.RobotsTxt, storedSetting.RobotsTxt);
                    Assert.Equal(testSetting.Disallow, storedSetting.Disallow);
                }
            }
        }
    }
}