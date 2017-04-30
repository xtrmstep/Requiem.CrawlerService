using System;
using System.Net;
using CrawlerService.Data;

namespace CrawlerService.Web.Impl
{
    /// <summary>
    ///     The wrapper around standard .NET web client
    /// </summary>
    internal class CrawlerWebClient : WebClient, ICrawlerWebClient
    {
        public string Download(string url)
        {
            var content = DownloadString(url);
            return content;
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