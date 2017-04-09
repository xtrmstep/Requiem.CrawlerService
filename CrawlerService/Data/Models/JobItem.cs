using System;

namespace CrawlerService.Data.Models
{
    public class JobItem
    {
        public Guid Id
        {
            get;
            set;
        }

        public UrlItem Url
        {
            get;
            set;
        }

        public DateTime DateStart
        {
            get;
            set;
        }

        public DateTime? DateFinish
        {
            get;
            set;
        }
    }
}