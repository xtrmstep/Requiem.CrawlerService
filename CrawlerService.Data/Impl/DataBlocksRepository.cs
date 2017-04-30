using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class DataBlocksRepository : IDataBlocksRepository
    {
        public Guid Add(string url, string data, string type)
        {
            using (var ctx = new CrawlerDbContext())
            {
                var dataBlock = new DataBlock {Data = data, Url = url, Type = type};
                ctx.DataBlocks.Add(dataBlock);
                ctx.SaveChanges();
                return dataBlock.Id;
            }
        }
    }
}