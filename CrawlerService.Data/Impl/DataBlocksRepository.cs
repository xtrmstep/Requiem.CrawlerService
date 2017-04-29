using System;
using System.Linq;
using CrawlerService.Data.Models;

namespace CrawlerService.Data.Impl
{
    internal class DataBlocksRepository : IDataBlocksRepository
    {
        private readonly IActivityLogRepository _logger;

        public DataBlocksRepository(IActivityLogRepository logger)
        {
            _logger = logger;
        }

        public Guid StoreData(Process jobItem, DataBlockType blockType, string data)
        {
            var url = string.Empty;
            try
            {
                using (var ctx = new CrawlerDbContext())
                {
                    var urlItem = ctx.DomainNames.Single(u => u.Id == jobItem.Domain.Id);
                    url = urlItem.Url;
                    var dataBlock = new DataBlock
                    {
                        Data = data,
                        Date = jobItem.DateStart,
                        Url = urlItem,
                        Type = blockType
                    };
                    ctx.DataBlocks.Add(dataBlock);

                    ctx.SaveChanges();

                    _logger.DataStored(jobItem);
                    return dataBlock.Id;
                }
            }
            catch (Exception err)
            {
                _logger.LogError(url, err);
                throw;
            }
        }
    }
}