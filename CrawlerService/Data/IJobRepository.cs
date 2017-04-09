using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    /// <summary>
    /// Crawler job descriptor
    /// </summary>
    /// <remarks>When the crawler is going to process an URL a new job should be associated with this process.
    /// All errors which occurred during the work should be added to the job for later analysis.
    /// </remarks>
    public interface IJobRepository
    {
        /// <summary>
        /// Create a new job
        /// </summary>
        /// <param name="urlItem"></param>
        /// <returns></returns>
        JobItem Start(UrlItem urlItem);

        /// <summary>
        /// The crawler is finished the work without errors or with errors which do not affect the result
        /// </summary>
        /// <param name="jobItem"></param>
        void Complete(JobItem jobItem);

        /// <summary>
        /// The crawler is stopped to work before all process is finished. See errors to find out the cause of the issue.
        /// </summary>
        /// <param name="jobItem"></param>
        void Stop(JobItem jobItem);
    }
}