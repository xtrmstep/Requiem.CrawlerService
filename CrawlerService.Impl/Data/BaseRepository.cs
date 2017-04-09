using CrawlerService.Data;

namespace CrawlerService.Impl.Data
{
    /// <summary>
    /// Provides routines common for all repositories
    /// </summary>
    internal abstract class BaseRepository
    {
        /// <summary>
        /// Create data context
        /// </summary>
        /// <returns></returns>
        protected ICrawlerDbContext CreateContext()
        {
            return new CrawlerDbContext();
        }
    }
}