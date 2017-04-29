namespace CrawlerService.Web
{
    /// <summary>
    ///     A class which implements the interface should create a web client to interact with web resources
    /// </summary>
    public interface IWebClientFactory
    {
        /// <summary>
        ///     Create a web client
        /// </summary>
        /// <returns></returns>
        ICrawlerWebClient CreateWebClient();
    }
}