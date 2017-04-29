using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    /// <summary>
    ///     Provides routines to manage data blocks
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        ///     Store found data blocks
        /// </summary>
        /// <param name="jobItem"></param>
        /// <param name="blockType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Guid StoreData(JobItem jobItem, DataBlockType blockType, string data);
    }
}