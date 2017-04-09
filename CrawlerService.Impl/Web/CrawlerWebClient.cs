using System;
using System.Net;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Web;

namespace CrawlerService.Impl.Web
{
    /// <summary>
    /// The wrapper around standard .NET web client
    /// </summary>
    internal class CrawlerWebClient : WebClient, ICrawlerWebClient
    {
        private readonly IActivityLogRepository _logger;

        public CrawlerWebClient(IActivityLogRepository logger)
        {
            _logger = logger;
        }

        public string Download(string url)
        {
            try
            {
                var content = DownloadString(url);
                _logger.UrlDownloaded(url);
                return content;
            }
            catch (Exception err)
            {
                _logger.LogError(url, err);
                throw;
            }
        }

        public string GetHeader(HttpRequestHeader headerName)
        {
            return Headers[headerName];
        }

        public void RemoveHeader(HttpRequestHeader headerName)
        {
            Headers.Remove(headerName);
        }

        public void SetHeader(HttpRequestHeader headerName, string value)
        {
            Headers.Set(headerName, value);
        }
    }
}