using System;
using System.Net;
using CrawlerService.Data.Models;

namespace CrawlerService.Web
{
    /// <summary>
    /// The interface for class which implements Web routines on low level
    /// </summary>
    public interface ICrawlerWebClient : IDisposable
    {
        /// <summary>
        /// Asynchronously download a web page 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string Download(string url);

        /// <summary>
        /// Set a header for all web requests
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="value"></param>
        void SetHeader(HttpRequestHeader headerName, string value);

        /// <summary>
        /// Get a value of a header for all web requests
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        string GetHeader(HttpRequestHeader headerName);

        /// <summary>
        /// Remove a header from all web requests
        /// </summary>
        /// <param name="headerName"></param>
        void RemoveHeader(HttpRequestHeader headerName);
    }
}