using Xunit;

namespace CrawlerService.Data
{
    [CollectionDefinition("DbIntegratedTests")]
    public class DbIntegratedTestsCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}