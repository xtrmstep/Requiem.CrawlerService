using System;
using System.Data.Entity;
using System.Linq;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class ProcessesRepository : IProcessesRepository
    {
        private readonly IActivityLogRepository _logger;

        public ProcessesRepository(IActivityLogRepository logger)
        {
            _logger = logger;
        }

        public Process Start(DomainName domain)
        {
            var jobCreationDateTime = DateTime.UtcNow;
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    // make sure there is no running job for the URL
                    var runningJob = ctx.Processes.Include(j => j.Domain).SingleOrDefault(j => j.Domain.Url == domain.Url && j.DateFinish.HasValue == false);
                    if (runningJob != null)
                    {
                        throw new Exception("Job is already running");
                    }

                    // refresh URL item with the tracked one to avoid PK duplication
                    var existingUrl = ctx.DomainNames.SingleOrDefault(u => u.Id == domain.Id) ?? domain;
                    var newJob = new Process
                    {
                        Id = Guid.NewGuid(), // todo get rid
                        DateStart = jobCreationDateTime,
                        Domain = existingUrl
                    };
                    ctx.Processes.Add(newJob);
                    ctx.SaveChanges();

                    _logger.JobStarted(newJob);
                    return newJob;
                }
            }
            catch (Exception err)
            {
                _logger.LogError(domain, err);
                throw;
            }
        }

        public void Complete(Process process)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    process = ctx.Processes
                        .Include(j => j.Domain)
                        .Single(j => j.Id == process.Id);
                    var logDate = DateTime.UtcNow;
                    process.Domain.IsInProgress = false;
                    process.DateFinish = logDate;
                    _logger.JobCompleted(process);
                    ctx.SaveChanges();
                }
            }
            catch (Exception err)
            {
                _logger.LogError(process, err);
                throw;
            }
        }

        public void Stop(Process jobItem)
        {
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    jobItem = ctx.Processes
                        .Include(j => j.Domain)
                        .Single(j => j.Id == jobItem.Id);
                    var logDate = DateTime.UtcNow;
                    jobItem.Domain.IsInProgress = false;
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