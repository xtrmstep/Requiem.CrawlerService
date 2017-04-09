using System;
using System.Data.Entity;
using System.Threading.Tasks;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    /// <summary>
    /// Data context of the crawler
    /// </summary>
    public interface ICrawlerDbContext : IDisposable
    {
        IDbSet<UrlItem> UrlItems
        {
            get;
            set;
        }

        IDbSet<HostSetting> HostSettings
        {
            get;
            set;
        }

        IDbSet<DataBlock> DataBlocks
        {
            get;
            set;
        }
    
        IDbSet<CrawlRule> CrawlRules
        {
            get;
            set;
        }

        IDbSet<JobItem> JobItems
        {
            get;
            set;
        }

        IDbSet<ActivityMessage> ActivityMessages
        {
            get;
            set;
        }

        Task CommitAsync();
        void Commit();
    }
}