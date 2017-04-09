using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public class DownloadedContentData : Tuple<JobItem, string>
    {
        public DownloadedContentData(JobItem job, string content)
            : base(job, content)
        {
        }

        public JobItem Job
        {
            get
            {
                return Item1;
            }
        }

        public string Content
        {
            get
            {
                return Item2;
            }
        }
    }
}