using Xunit;

namespace CrawlerService.Impl.Tests
{
    [CollectionDefinition("DbIntegratedTests")]
    public class DbIntegratedTestsCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}