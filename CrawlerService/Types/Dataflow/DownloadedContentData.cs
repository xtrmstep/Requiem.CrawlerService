using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public class DownloadedContentData : Tuple<Process, string>
    {
        public DownloadedContentData(Process job, string content)
            : base(job, content)
        {
        }

        public Process Job
        {
            get { return Item1; }
        }

        public string Content
        {
            get { return Item2; }
        }
    }
}