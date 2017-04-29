using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public class ParsingRulesData : Tuple<JobItem, CrawlRule, string>
    {
        public ParsingRulesData(JobItem job, CrawlRule rule, string content)
            : base(job, rule, content)
        {
        }

        public JobItem Job
        {
            get { return Item1; }
        }

        public CrawlRule Rule
        {
            get { return Item2; }
        }

        public string Content
        {
            get { return Item3; }
        }
    }
}