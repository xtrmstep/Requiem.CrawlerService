using System.Collections.Generic;
using System.Text.RegularExpressions;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Types;
using CrawlerService.Types.Dataflow;
using CrawlerService.Web;

namespace CrawlerService.Impl.Types.Dataflow
{
    class PipelineRoutines : IPipeline
    {
        public DownloadedContentData DownloadContent(JobItem job)
        {
            var web = ServiceLocator.Resolve<IWebClientFactory>();
            
            using (var client = web.CreateWebClient())
            {
                var url = job.Url.Url;
                var content = client.Download(url);
                return new DownloadedContentData(job, content);
            }
        }

        public IEnumerable<ParsingRulesData> GetParsingRules(DownloadedContentData data)
        {
            var settings = ServiceLocator.Resolve<ICrawlerSettingsRepository>();

            var rules = settings.GetParsingRules(data.Job);
            foreach (var rule in rules)
            {
                yield return new ParsingRulesData(data.Job, rule, data.Content);
            }
        }

        public IEnumerable<ParsedContentData> ParseContent(ParsingRulesData data)
        {
            var matches = Regex.Matches(data.Content, data.Rule.RegExpression);
            foreach (Match match in matches)
            {
                yield return new ParsedContentData(data.Job, data.Rule.DataType, match.Value);
            }
        }

        public JobItem StoreData(ParsedContentData data)
        {
            var datas = ServiceLocator.Resolve<IDataRepository>();

            datas.StoreData(data.Job, data.BlockType, data.Data);

            return data.Job;
        }
    }
}