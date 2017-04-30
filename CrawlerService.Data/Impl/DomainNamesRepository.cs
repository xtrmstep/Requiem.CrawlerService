using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CrawlerService.Data.Models;
using CrawlerService.Data.Types;

namespace CrawlerService.Data.Impl
{
    internal class DomainNamesRepository : IDomainNamesRepository
    {
        private readonly IMapper _mapper;

        public DomainNamesRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DomainName GetNextDomain(DateTime asOfDate)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var availableDomain = ctx.DomainNames.Include(d => d.Processes)
                    .Where(d => d.Processes.Any(p => p.Status == Statuses.IN_PROGRESS) == false) // no running processes
                    .Where(d => !d.EvaliableFromDate.HasValue || DbFunctions.DiffSeconds(d.EvaliableFromDate, asOfDate) < 0) // EvaliableFromDate is earlier than asOfDate
                    .OrderByDescending(d => d.EvaliableFromDate)
                    .AsNoTracking()
                    .FirstOrDefault();
                return availableDomain;
            }
        }

        public void Update(DomainName domain)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var trackedDomain = ctx.DomainNames.Single(d => d.Name == domain.Name);
                _mapper.Map(domain, trackedDomain);
                ctx.SaveChanges();
            }
        }
    }
}