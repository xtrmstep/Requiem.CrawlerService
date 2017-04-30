using Xunit;

namespace CrawlerService.Data.Fixtures
{
    [CollectionDefinition("DbIntegratedTests")]
    public class DbIntegratedTestsCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}