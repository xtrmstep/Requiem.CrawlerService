using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    /// <summary>
    ///     Crawler process
    /// </summary>
    public interface IProcessesRepository
    {
        Process Start(DomainName domain);
        void Complete(Process process);
        void Update(Process process);
    }
}