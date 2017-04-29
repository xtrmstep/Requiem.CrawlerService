using System;
using System.Data.Entity;
using System.Linq;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class JobRepository : IJobRepository
    {
        private readonly IActivityLogRepository _logger;

        public JobRepository(IActivityLogRepository logger)
        {
            _logger = logger;
        }

        public JobItem Start(UrlItem urlItem)
        {
            var jobCreationDateTime = DateTime.UtcNow;
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    // make sure there is no running job for the URL
                    var runningJob = ctx.JobItems.Include(j => j.Url).SingleOrDefault(j => j.Url.Url == urlItem.Url && j.DateFinish.HasValue == false);
                    if (runningJob != null)
                    {
                        throw new Exception("Job is already running");
                    }

                    // refresh URL item with the tracked one to avoid PK duplication
                    var existingUrl = ctx.UrlItems.SingleOrDefault(u => u.Id == urlItem.Id) ?? urlItem;
                    var newJob = new JobItem
                    {
                        Id = Guid.NewGuid(), // todo get rid
                        DateStart = jobCreationDateTime,
                        Url = existingUrl
                    };
                    ctx.JobItems.Add(newJob);
                    ctx.SaveChanges();

                    _logger.JobStarted(newJob);
                    return newJob;
                }
            }
            catch (Exception err)
            {
                _logger.LogError(urlItem, err);
                throw;
            }
        }

        public void Complete(JobItem jobItem)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    jobItem = ctx.JobItems
                        .Include(j => j.Url)
                        .Single(j => j.Id == jobItem.Id);
                    var logDate = DateTime.UtcNow;
                    jobItem.Url.IsInProgress = false;
                    jobItem.DateFinish = logDate;
                    _logger.JobCompleted(jobItem);
                    ctx.SaveChanges();
                }
            }
            catch (Exception err)
            {
                _logger.LogError(jobItem, err);
                throw;
            }
        }

        public void Stop(JobItem jobItem)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    jobItem = ctx.JobItems
                        .Include(j => j.Url)
                        .Single(j => j.Id == jobItem.Id);
                    var logDate = DateTime.UtcNow;
                    jobItem.Url.IsInProgress = false;
                    jobItem.DateFinish = logDate;
                    _logger.JobStopped(jobItem);
                    ctx.SaveChanges();
                }
            }
            catch (Exception err)
            {
                _logger.LogError(jobItem, err);
                throw;
            }
        }
    }
}