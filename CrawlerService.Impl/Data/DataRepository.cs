using System;
using System.Linq;
using CrawlerService.Data;
using CrawlerService.Data.Models;

namespace CrawlerService.Impl.Data
{
    internal class DataRepository : BaseRepository, IDataRepository
    {
        private IActivityLogRepository _logger;

        public DataRepository(IActivityLogRepository logger)
        {
            _logger = logger;
        }

        public Guid StoreData(JobItem jobItem, DataBlockType blockType, string data)
        {
            var url = string.Empty;
            try
            {
                using (var ctx = CreateContext())
                {
                    var urlItem = ctx.UrlItems.Single(u => u.Id == jobItem.Url.Id);
                    url = urlItem.Url;
                    var dataBlock = new DataBlock
                    {
                        Data = data,
                        Date = jobItem.DateStart,
                        Url = urlItem,
                        Type = blockType
                    };
                    ctx.DataBlocks.Add(dataBlock);

                    ctx.Commit();

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