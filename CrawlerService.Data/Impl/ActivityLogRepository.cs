using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class ActivityLogRepository : IActivityLogRepository
    {
        public void UrlDownloaded(string url)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.UrlDownloaded, url)
                });
                ctx.SaveChanges();
            }
        }

        public void RulesTaken(JobItem job)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.RulesTaken, job.Id)
                });
                ctx.SaveChanges();
            }
        }

        public void DataStored(JobItem job)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.DataStored, job.Id)
                });
                ctx.SaveChanges();
            }
        }

        private struct LogMessages
        {
            public const string JobStarted = "Job is started '{0}'";
            public const string JobCompleted = "Job is completed '{0}'";
            public const string JobStopped = "Job is stopped for '{0}'";
            public const string UrlDownloaded = "URL is downloaded: {0}";
            public const string DataStored = "Data stored for job '{0}'";
            public const string RulesTaken = "Crawl rules are taken for job '{0}'";
            public const string LogErrorUrlText = "Error URL {0} : {1}";
            public const string LogErrorUrlItem = "Error URL '{0}' : {1}";
            public const string LogErrorJobItem = "Error JOB '{0}' : {1}";
            public const string SettingsStored = "Settings are stored for '{0}'";
            public const string SettingsLoaded = "Settings are loaded for '{0}'";
        }

        #region Errors

        public void LogError(string url, Exception err)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Type = ActivityMessageType.Error,
                    Message = string.Format(LogMessages.LogErrorUrlText, url, err)
                });
                ctx.SaveChanges();
            }
        }

        public void LogError(UrlItem urlItem, Exception err)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Type = ActivityMessageType.Error,
                    Message = string.Format(LogMessages.LogErrorUrlItem, urlItem.Id, err)
                });
                ctx.SaveChanges();
            }
        }

        public void LogError(JobItem job, Exception err)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Type = ActivityMessageType.Error,
                    Message = string.Format(LogMessages.LogErrorJobItem, job.Id, err)
                });
                ctx.SaveChanges();
            }
        }

        #endregion

        #region Settings

        public void SettingsStored(UrlItem url)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.SettingsStored, url)
                });
                ctx.SaveChanges();
            }
        }

        public void SettingsLoaded(UrlItem url)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.SettingsLoaded, url)
                });
                ctx.SaveChanges();
            }
        }

        #endregion

        #region Jobs

        public void JobStarted(JobItem job)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.JobStarted, job.Id)
                });
                ctx.SaveChanges();
            }
        }

        public void JobCompleted(JobItem job)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.JobCompleted, job.Id)
                });
                ctx.SaveChanges();
            }
        }

        public void JobStopped(JobItem job)
        {
            using (var ctx = new CrawlerDbContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.JobStopped, job.Id)
                });
                ctx.SaveChanges();
            }
        }

        #endregion
    }
}