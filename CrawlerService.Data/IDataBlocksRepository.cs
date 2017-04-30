using System;

namespace CrawlerService.Data
{
    public interface IDataBlocksRepository
    {
        Guid Add(string url, string data, string type);
    }
}