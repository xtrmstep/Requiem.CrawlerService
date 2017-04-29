using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public class ParsingRulesData : Tuple<Process, ExtractRule, string>
    {
        public ParsingRulesData(Process job, ExtractRule rule, string content)
            : base(job, rule, content)
        {
        }

        public Process Job
        {
            get { return Item1; }
        }

        public ExtractRule Rule
        {
            get { return Item2; }
        }

        public string Content
        {
            get { return Item3; }
        }
    }
}