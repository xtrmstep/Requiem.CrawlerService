using System.Collections.Generic;
using System.Text.RegularExpressions;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Web;

namespace CrawlerService.Types.Dataflow.Impl
{
    internal class PipelineRoutines : IPipeline
    {
        private readonly ICrawlerSettingsRepository _crawlerSettingsRepository;
        private readonly IDataBlocksRepository _dataRepository;
        private readonly IWebClientFactory _webClientFactory;

        public PipelineRoutines(IWebClientFactory webClientFactory, ICrawlerSettingsRepository crawlerSettingsRepository, IDataBlocksRepository dataRepository)
        {
            _webClientFactory = webClientFactory;
            _crawlerSettingsRepository = crawlerSettingsRepository;
            _dataRepository = dataRepository;
        }

        public DownloadedContentData DownloadContent(Process job)
        {
            using (var client = _webClientFactory.CreateWebClient())
            {
                var url = job.Domain.Url;
                var content = client.Download(url);
                return new DownloadedContentData(job, content);
            }
        }

        public IEnumerable<ParsingRulesData> GetParsingRules(DownloadedContentData data)
        {
            var rules = _crawlerSettingsRepository.GetParsingRules(data.Job);
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

        public Process StoreData(ParsedContentData data)
        {
            _dataRepository.StoreData(data.Job, data.BlockType, data.Data);

            return data.Job;
        }
    }
}