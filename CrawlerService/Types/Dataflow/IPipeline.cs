using System.Collections.Generic;
using CrawlerService.Data.Models;

namespace CrawlerService.Types.Dataflow
{
    public interface IPipeline
    {
        /// <summary>
        /// Download content from URL associated with the job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        DownloadedContentData DownloadContent(JobItem job);

        /// <summary>
        /// Get parsing rules
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerable<ParsingRulesData> GetParsingRules(DownloadedContentData data);

        /// <summary>
        /// Parse content and find all matching text blocks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerable<ParsedContentData> ParseContent(ParsingRulesData data);

        /// <summary>
        /// Store data to data warehouse
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        JobItem StoreData(ParsedContentData data);
    }
}