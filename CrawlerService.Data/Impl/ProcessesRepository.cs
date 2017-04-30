using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CrawlerService.Common.DateTime;
using CrawlerService.Data.Models;
using CrawlerService.Data.Types;

namespace CrawlerService.Data.Impl
{
    internal class ProcessesRepository : IProcessesRepository
    {
        private readonly IMapper _mapper;

        public ProcessesRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Process Start(DomainName domain)
        {
            using (var ctx = new CrawlerDbContext(IsolationLevel.Serializable))
            {
                var isAlreadyInProgress = ctx.Processes.Include(p => p.Domain)
                    .Any(p => p.Status == Statuses.IN_PROGRESS && p.Domain.Name == domain.Name);

                if (isAlreadyInProgress)
                    throw new Exception("Already in progress");

                var newProcess = new Process
                {
                    Domain = domain,
                    Status = Statuses.IN_PROGRESS
                };
                ctx.Processes.Add(newProcess);
                ctx.SaveChanges();
                return newProcess;
            }
        }

        public void Complete(Process process)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var trackedProcess = ctx.Processes.Single(p => p.Id == process.Id);
                trackedProcess.DateFinish = CrawlerDateTime.Now;
                trackedProcess.Status = Statuses.FINISHED;
                ctx.SaveChanges();
            }
        }

        public void Update(Process process)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var trackedProcess = ctx.Processes.Single(p => p.Id == process.Id);
                _mapper.Map(process, trackedProcess);
                ctx.SaveChanges();
            }
        }
    }
}