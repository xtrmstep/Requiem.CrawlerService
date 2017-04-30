using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    public interface IDomainNamesRepository
    {
        DomainName GetNextDomain(DateTime asOfDate);
        void Update(DomainName domain);
    }
}