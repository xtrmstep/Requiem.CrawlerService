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
            var urlFrontierRepository = Mock.Of<IUrlFrontierRepository>();
            var jobRepository = Mock.Of<IJobRepository>();
            var pipeline = Mock.Of<IPipeline>();
            var runner = new Runner(urlFrontierRepository, jobRepository, pipeline);

            #region mocks configuration

            #region mock returns

            var jobItem = new JobItem();
            var downloadedContentData = new DownloadedContentData(jobItem, string.Empty);
            var crawlRule = new CrawlRule();
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
                new UrlItem()
            };

            #endregion

            #region pipeline mock

            var mockPipeline = new Mock<IPipeline>();
            mockPipeline.Setup(m => m.DownloadContent(It.IsAny<JobItem>())).Returns(downloadedContentData);
            mockPipeline.Setup(m => m.GetParsingRules(It.IsAny<DownloadedContentData>())).Returns(parsingRulesDatas);
            mockPipeline.Setup(m => m.ParseContent(It.IsAny<ParsingRulesData>())).Returns(parsedContentDatas);
            mockPipeline.Setup(m => m.StoreData(It.IsAny<ParsedContentData>())).Returns(jobItem);

            #endregion

            #region frontier mock

            // at least one URL should be exist to allow the downloading of content
            var mockFrontier = new Mock<IUrlFrontierRepository>();
            mockFrontier.Setup(m => m.GetAvailableUrls(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(urlItems);

            #endregion

            #region jobs mock

            // job should be created for the URL
            var mockJob = new Mock<IJobRepository>();
            mockJob.Setup(m => m.Start(It.IsAny<UrlItem>())).Returns(jobItem);

            #endregion

            #region settings mock

            // at least one rule should be exist to allow the downloading of content
            var mockSettings = new Mock<ICrawlerSettingsRepository>();
            mockSettings.Setup(m => m.GetParsingRules(It.IsAny<JobItem>())).Returns(crawlRules);

            #endregion

            #endregion

            runner.Run();

            mockPipeline.Verify(m => m.DownloadContent(It.IsAny<JobItem>()), Times.Once);
            mockPipeline.Verify(m => m.GetParsingRules(It.IsAny<DownloadedContentData>()), Times.Once);
            mockPipeline.Verify(m => m.ParseContent(It.IsAny<ParsingRulesData>()), Times.Once);
            mockPipeline.Verify(m => m.StoreData(It.IsAny<ParsedContentData>()), Times.Once);
        }
    }
}