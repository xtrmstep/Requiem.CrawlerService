using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public class ParsedContentData : Tuple<Process, string, string>
    {
        public ParsedContentData(Process job, string blockType, string data)
            : base(job, blockType, data)
        {
        }

        public Process Job
        {
            get { return Item1; }
        }

        public string BlockType
        {
            get { return Item2; }
        }

        public string Data
        {
            get { return Item3; }
        }
    }
}