using System;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public class ParsedContentData : Tuple<JobItem, DataBlockType, string>
    {
        public ParsedContentData(JobItem job, DataBlockType blockType, string data)
            : base(job, blockType, data)
        {
        }

        public JobItem Job
        {
            get
            {
                return Item1;
            }
        }

        public DataBlockType BlockType
        {
            get
            {
                return Item2;
            }
        }

        public string Data
        {
            get
            {
                return Item3;
            }
        }
    }
}