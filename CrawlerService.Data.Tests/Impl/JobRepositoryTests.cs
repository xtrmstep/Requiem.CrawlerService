using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using CrawlerService.Data.Models;
using Moq;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class JobRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public JobRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "JobRepository should create new job for new url")]
        public void Should_create_new_job_for_new_url()
        {
            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };
                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var job = jobRep.Start(urlItem);

                #endregion

                #region assertion

                // make sure the job is created in DB
                using (var ctx = _db.CreateDbContext())
                {
                    // try to find unfinished job for the url
                    var actualJob = ctx.Processes.Include(j => j.Domain).SingleOrDefault(j => j.DateFinish.HasValue == false);
                    Assert.NotNull(actualJob);
                    Assert.Equal(job.Id, actualJob.Id);
                    Assert.Equal(testUrl, actualJob.Domain.Url);
                    Assert.False(actualJob.DateFinish.HasValue);
                }

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should create new job for old url")]
        public void Should_create_new_job_for_url_which_has_inactive_job()
        {
            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };

                // add finished job
                using (var ctx = _db.CreateDbContext())
                {
                    ctx.Processes.Add(new Process
                    {
                        Id = Guid.NewGuid(),
                        DateStart = DateTime.Now.AddDays(-2),
                        DateFinish = DateTime.Now.AddDays(-2), // make sure the job is finished
                        Domain = urlItem
                    });
                    ctx.SaveChanges();
                }

                #region action

                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var job = jobRep.Start(urlItem);

                #endregion

                #region assertion

                // make sure the job is created in DB
                using (var ctx = _db.CreateDbContext())
                {
                    // try to find unfinished job for the url
                    var actualJob = ctx.Processes.Include(j => j.Domain).SingleOrDefault(j => j.DateFinish.HasValue == false);
                    Assert.NotNull(actualJob);
                    Assert.Equal(job.Id, actualJob.Id);
                    Assert.Equal(testUrl, actualJob.Domain.Url);
                    Assert.False(actualJob.DateFinish.HasValue);
                }

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should not create new job if already exists one")]
        public void Should_return_null_if_job_is_already_running_for_url()
        {
            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };

                // add not finished job
                using (var ctx = _db.CreateDbContext())
                {
                    ctx.Processes.Add(new Process
                    {
                        Id = Guid.NewGuid(),
                        DateStart = DateTime.Now.AddDays(-2),
                        DateFinish = null, // make sure the job is NOT finished
                        Domain = urlItem
                    });
                    ctx.SaveChanges();
                }

                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                Assert.Throws<Exception>(() => jobRep.Start(urlItem));
            }
        }

        [Fact(DisplayName = "JobRepository should add log message when job is started")]
        public void Should_add_log_message_when_job_is_started()
        {
            var mockLogger = new Mock<IActivityLogRepository>();

            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };
                var jobRep = new ProcessesRepository(mockLogger.Object);
                jobRep.Start(urlItem);

                #endregion

                #region assertion

                mockLogger.Verify(m => m.JobStarted(It.IsAny<Process>()), Times.Once);

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should add log message when job is completed")]
        public void Should_add_item_to_history_when_job_is_completed()
        {
            var mockLogger = new Mock<IActivityLogRepository>();

            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };

                var jobRep = new ProcessesRepository(mockLogger.Object);
                var job = jobRep.Start(urlItem); // adds the 1st log message
                // wait a little bit to force difference in time between two log messages and sort them later
                Thread.Sleep(100);
                jobRep.Complete(job); // adds the 2nd log message

                #endregion

                #region assertion

                mockLogger.Verify(m => m.JobStarted(It.IsAny<Process>()), Times.Once);
                mockLogger.Verify(m => m.JobCompleted(It.IsAny<Process>()), Times.Once);

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should mark job finished when it is completed")]
        public void Should_mark_job_finished_when_it_is_completed()
        {
            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };
                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var job = jobRep.Start(urlItem); // adds the 1st log message
                // wait a little bit to force difference in time between two log messages and sort them later
                Thread.Sleep(100);
                jobRep.Complete(job); // marks the job finished

                #endregion

                #region assertion

                // make sure the job is created in DB
                using (var ctx = _db.CreateDbContext())
                {
                    var finishedJob = ctx.Processes.Single(j => j.Id == job.Id);
                    Assert.True(finishedJob.DateFinish.HasValue);
                }

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should add log message when job is stopped")]
        public void Should_add_item_to_history_when_job_is_stopped()
        {
            var mockLogger = new Mock<IActivityLogRepository>();

            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };
                var jobRep = new ProcessesRepository(mockLogger.Object);
                var job = jobRep.Start(urlItem); // adds the 1st log message
                // wait a little bit to force difference in time between two log messages and sort them later
                Thread.Sleep(100);
                jobRep.Stop(job); // adds the 2nd log message

                #endregion

                #region assertion

                mockLogger.Verify(m => m.JobStarted(It.IsAny<Process>()), Times.Once);
                mockLogger.Verify(m => m.JobStopped(It.IsAny<Process>()), Times.Once);

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should mark job finished when it is stopped")]
        public void Should_mark_job_finished_when_it_is_stopped()
        {
            const string testUrl = "http://test.com";
            const string testHost = "test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var urlItem = new DomainName
                {
                    Url = testUrl,
                    Host = testHost
                };
                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var job = jobRep.Start(urlItem); // adds the 1st log message
                // wait a little bit to force difference in time between two log messages and sort them later
                Thread.Sleep(100);
                jobRep.Stop(job); // marks the job finished

                #endregion

                #region assertion

                // make sure the job is created in DB
                using (var ctx = _db.CreateDbContext())
                {
                    var finishedJob = ctx.Processes.Single(j => j.Id == job.Id);
                    Assert.True(finishedJob.DateFinish.HasValue);
                }

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should release URL when job is completed")]
        public void Should_release_URL_when_job_is_completed()
        {
            const string testUrl = "http://test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var frontier = new DomainNamesRepository();
                var nextAvailableTime = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc); // already available
                frontier.AddOrUpdateUrl(testUrl, nextAvailableTime);
                var urlItem = frontier.GetAvailableUrls(1, DateTime.UtcNow).First(); // should return one item

                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var job = jobRep.Start(urlItem); // adds the 1st log message
                // wait a little bit to force difference in time between two log messages and sort them later
                Thread.Sleep(100);
                jobRep.Complete(job); // marks the job finished

                #endregion

                #region assertion

                // make sure the URL is not in progress
                using (var ctx = _db.CreateDbContext())
                {
                    urlItem = ctx.DomainNames.Single(url => url.Id == urlItem.Id);
                    Assert.False(urlItem.IsInProgress);
                }

                #endregion
            }
        }

        [Fact(DisplayName = "JobRepository should release URL when job  is stopped")]
        public void Should_release_URL_when_job_is_stopped()
        {
            const string testUrl = "http://test.com";

            using (_db.CreateTransaction())
            {
                #region action

                var frontier = new DomainNamesRepository();
                var nextAvailableTime = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc); // already available
                frontier.AddOrUpdateUrl(testUrl, nextAvailableTime);
                var urlItem = frontier.GetAvailableUrls(1, DateTime.UtcNow).First(); // should return one item

                var jobRep = new ProcessesRepository(Mock.Of<IActivityLogRepository>());
                var job = jobRep.Start(urlItem); // adds the 1st log message
                // wait a little bit to force difference in time between two log messages and sort them later
                Thread.Sleep(100);
                jobRep.Stop(job); // marks the job finished

                #endregion

                #region assertion

                // make sure the URL is not in progress
                using (var ctx = _db.CreateDbContext())
                {
                    urlItem = ctx.DomainNames.Single(url => url.Id == urlItem.Id);
                    Assert.False(urlItem.IsInProgress);
                }

                #endregion
            }
        }
    }
}