using System;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Types.Dataflow;
using Moq;
using Xunit;

namespace CrawlerService.Core
{
    public class RunnerTests : IClassFixture<DatabaseFixture>
    {
        [Fact(DisplayName = "Should call pipeline methods")]
        public void Should_call_pipeline_methods()
        {
            var urlFrontierRepository = Mock.Of<IDomainNamesRepository>();
            var jobRepository = Mock.Of<IProcessesRepository>();
            var pipeline = Mock.Of<IPipeline>();
            var runner = new Runner(urlFrontierRepository, jobRepository, pipeline);

            #region mocks configuration

            #region mock returns

            var jobItem = new Process();
            var downloadedContentData = new DownloadedContentData(jobItem, string.Empty);
            var crawlRule = new ExtractRule();
            var parsingRulesDatas = new[]
            {
                new ParsingRulesData(jobItem, crawlRule, string.Empty)
            };
            var parsedContentDatas = new[]
            {
                new ParsedContentData(jobItem, DataBlockType.Link, string.Empty)
            };
            var crawlRules = new[]
            {
                crawlRule
            };
            var urlItems = new[]
            {
                new DomainName()
            };

            #endregion

            #region pipeline mock

            var mockPipeline = new Mock<IPipeline>();
            mockPipeline.Setup(m => m.DownloadContent(It.IsAny<Process>())).Returns(downloadedContentData);
            mockPipeline.Setup(m => m.GetParsingRules(It.IsAny<DownloadedContentData>())).Returns(parsingRulesDatas);
            mockPipeline.Setup(m => m.ParseContent(It.IsAny<ParsingRulesData>())).Returns(parsedContentDatas);
            mockPipeline.Setup(m => m.StoreData(It.IsAny<ParsedContentData>())).Returns(jobItem);

            #endregion

            #region frontier mock

            // at least one URL should be exist to allow the downloading of content
            var mockFrontier = new Mock<IDomainNamesRepository>();
            mockFrontier.Setup(m => m.GetAvailableUrls(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(urlItems);

            #endregion

            #region jobs mock

            // job should be created for the URL
            var mockJob = new Mock<IProcessesRepository>();
            mockJob.Setup(m => m.Start(It.IsAny<DomainName>())).Returns(jobItem);

            #endregion

            #region settings mock

            // at least one rule should be exist to allow the downloading of content
            var mockSettings = new Mock<ICrawlerSettingsRepository>();
            mockSettings.Setup(m => m.GetParsingRules(It.IsAny<Process>())).Returns(crawlRules);

            #endregion

            #endregion

            runner.Run();

            mockPipeline.Verify(m => m.DownloadContent(It.IsAny<Process>()), Times.Once);
            mockPipeline.Verify(m => m.GetParsingRules(It.IsAny<DownloadedContentData>()), Times.Once);
            mockPipeline.Verify(m => m.ParseContent(It.IsAny<ParsingRulesData>()), Times.Once);
            mockPipeline.Verify(m => m.StoreData(It.IsAny<ParsedContentData>()), Times.Once);
        }
    }
}