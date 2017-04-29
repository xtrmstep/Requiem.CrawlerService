using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    public interface IDataBlocksRepository
    {
        Guid Add(string url, string data, string type);
    }
}