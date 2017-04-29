using System;
using System.Collections.Generic;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    public interface IDomainNamesRepository
    {
        DomainName GetNextDomain(DateTime asOfDate);

        void MoveAvailabilityDate(DomainName domain);

        void Update(DomainName domain);
    }
}