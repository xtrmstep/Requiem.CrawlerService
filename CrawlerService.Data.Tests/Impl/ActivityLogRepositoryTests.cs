using System;
using System.Linq;
using CrawlerService.Data.Models;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class ActivityLogRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public ActivityLogRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "UrlDownloaded should add message")]
        public void UrlDownloaded_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.UrlDownloaded("URL");

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "RulesTaken should add message")]
        public void RulesTaken_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.RulesTaken(new JobItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "DataStored should add message")]
        public void DataStored_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.DataStored(new JobItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "LogError should add message for URL text")]
        public void LogError_should_add_message_for_URL_text()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.LogError("URL", new Exception());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "LogError should add message for URL item")]
        public void LogError_should_add_message_for_URL_item()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.LogError(new UrlItem(), new Exception());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "LogError should add message for job")]
        public void LogError_should_add_message_for_job()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.LogError(new JobItem(), new Exception());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "SettingsStored should add message")]
        public void SettingsStored_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.SettingsStored(new UrlItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "SettingsLoaded should add message")]
        public void SettingsLoaded_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.SettingsLoaded(new UrlItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "JobStarted should add message")]
        public void JobStarted_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.JobStarted(new JobItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "JobCompleted should add message")]
        public void JobCompleted_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.JobCompleted(new JobItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }

        [Fact(DisplayName = "JobStopped should add message")]
        public void JobStopped_should_add_message()
        {
            using (_db.CreateTransaction())
            {
                var logger = new ActivityLogRepository();
                logger.JobStopped(new JobItem());

                using (var ctx = _db.CreateDbContext())
                {
                    var numberOfMessages = ctx.ActivityMessages.Count();
                    Assert.Equal(1, numberOfMessages);
                }
            }
        }
    }
}