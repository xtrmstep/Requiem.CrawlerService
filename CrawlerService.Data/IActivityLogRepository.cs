using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    public interface IActivityLogRepository
    {
        void UrlDownloaded(string url);
        void RulesTaken(JobItem job);
        void DataStored(JobItem job);

        #region Errors

        void LogError(string url, Exception err);
        void LogError(UrlItem urlItem, Exception err);
        void LogError(JobItem job, Exception err);

        #endregion

        #region Settings

        void SettingsStored(UrlItem url);
        void SettingsLoaded(UrlItem url);

        #endregion

        #region Jobs

        void JobStarted(JobItem job);
        void JobCompleted(JobItem job);
        void JobStopped(JobItem job);

        #endregion
    }
}