using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrawlerService.Data;
using CrawlerService.Data.Models;

namespace CrawlerService.Impl.Data
{
    class ActivityLogRepository : BaseRepository, IActivityLogRepository
    {
        public void UrlDownloaded(string url)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage {
                    Message = string.Format(LogMessages.UrlDownloaded, url)
                });
                ctx.Commit();
            }
        }

        public void RulesTaken(JobItem job)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.RulesTaken, job.Id)
                });
                ctx.Commit();
            }
        }

        public void DataStored(JobItem job)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.DataStored, job.Id)
                });
                ctx.Commit();
            }
        }

        #region Errors

        public void LogError(string url, Exception err)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Type = ActivityMessageType.Error,
                    Message = string.Format(LogMessages.LogErrorUrlText, url, err)
                });
                ctx.Commit();
            }
        }

        public void LogError(UrlItem urlItem, Exception err)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Type = ActivityMessageType.Error,
                    Message = string.Format(LogMessages.LogErrorUrlItem, urlItem.Id, err)
                });
                ctx.Commit();
            }
        }

        public void LogError(JobItem job, Exception err)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Type = ActivityMessageType.Error,
                    Message = string.Format(LogMessages.LogErrorJobItem, job.Id, err)
                });
                ctx.Commit();
            }
        }

        #endregion

        #region Settings

        public void SettingsStored(UrlItem url)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.SettingsStored, url)
                });
                ctx.Commit();
            }
        }

        public void SettingsLoaded(UrlItem url)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.SettingsLoaded, url)
                });
                ctx.Commit();
            }
        }

        #endregion

        #region Jobs

        public void JobStarted(JobItem job)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.JobStarted, job.Id)
                });
                ctx.Commit();
            }
        }

        public void JobCompleted(JobItem job)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.JobCompleted, job.Id)
                });
                ctx.Commit();
            }
        }

        public void JobStopped(JobItem job)
        {
            using (var ctx = CreateContext())
            {
                ctx.ActivityMessages.Add(new ActivityMessage
                {
                    Message = string.Format(LogMessages.JobStopped, job.Id)
                });
                ctx.Commit();
            }
        }

        #endregion

        struct LogMessages
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
    }
}
