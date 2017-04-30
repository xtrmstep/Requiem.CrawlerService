﻿using System;
using System.Linq;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Web;
using Moq;
using Xunit;

namespace CrawlerService.Types.Dataflow.Impl
{
    public class PipelineRoutinesTests
    {
        [Fact(DisplayName = "Download content")]
        public void Should_download_content()
        {
            #region arrange data

            var mockClient = new Mock<ICrawlerWebClient>();
            const string testUrl = "url";
            const string testContent = "text";
            mockClient.Setup(m => m.Download(It.IsAny<string>())).Returns(testContent);

            var jobItem = new Process
            {
                Domain = new DomainName
                {
                    Url = testUrl
                }
            };

            #endregion

            var webClientFactory = Mock.Of<IWebClientFactory>();
            var crawlerSettingsRepository = Mock.Of<ICrawlerSettingsRepository>();
            var dataRepository = Mock.Of<IDataBlocksRepository>();
            var pipelineRoutines = new PipelineRoutines(webClientFactory, crawlerSettingsRepository, dataRepository);
            var actual = pipelineRoutines.DownloadContent(jobItem);

            Assert.NotNull(actual);
            Assert.Equal(testContent, actual.Content);
            Assert.Equal(jobItem, actual.Job);
            Assert.Equal(testUrl, actual.Job.Domain.Url);
        }

        [Fact(DisplayName = "Return parsing rules")]
        public void Should_return_parsing_rules()
        {
            #region arrange data

            var mockSettings = new Mock<ICrawlerSettingsRepository>();
            const string testUrl = "url";
            const string testContent = "text";
            mockSettings.Setup(m => m.GetParsingRules(It.IsAny<Process>())).Returns(new[]
            {
                new ExtractRule {DataType = DataBlockType.Link},
                new ExtractRule {DataType = DataBlockType.Picture}
            });

            var jobItem = new Process
            {
                Domain = new DomainName
                {
                    Url = testUrl
                }
            };

            #endregion

            var webClientFactory = Mock.Of<IWebClientFactory>();
            var crawlerSettingsRepository = Mock.Of<ICrawlerSettingsRepository>();
            var dataRepository = Mock.Of<IDataBlocksRepository>();
            var pipelineRoutines = new PipelineRoutines(webClientFactory, crawlerSettingsRepository, dataRepository);
            var actual = pipelineRoutines.GetParsingRules(new DownloadedContentData(jobItem, testContent)).ToList();

            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count);
            Assert.Equal(testContent, actual[0].Content);
            Assert.Equal(testContent, actual[1].Content);
            Assert.Equal(jobItem, actual[0].Job);
            Assert.Equal(jobItem, actual[1].Job);
            Assert.Equal(DataBlockType.Link, actual[0].Rule.DataType);
            Assert.Equal(DataBlockType.Picture, actual[1].Rule.DataType);
        }

        [Fact(DisplayName = "Return all matching text block when parsing")]
        public void Should_return_all_matching_text_block_when_parsing()
        {
            #region arrange data

            const string testUrl = "url";
            const string testContent = "<a>text [text0] should be found in [text1] square [text2]brackets</a>";
            var jobItem = new Process
            {
                Domain = new DomainName
                {
                    Url = testUrl
                }
            };

            var crawlRule = new ExtractRule {DataType = DataBlockType.Link, RegExpression = @"\[\w+\d{1}\]"};

            #endregion

            var webClientFactory = Mock.Of<IWebClientFactory>();
            var crawlerSettingsRepository = Mock.Of<ICrawlerSettingsRepository>();
            var dataRepository = Mock.Of<IDataBlocksRepository>();
            var pipelineRoutines = new PipelineRoutines(webClientFactory, crawlerSettingsRepository, dataRepository);
            var actual = pipelineRoutines.ParseContent(new ParsingRulesData(jobItem, crawlRule, testContent)).ToList();

            Assert.NotNull(actual);
            Assert.Equal(3, actual.Count);

            Assert.Equal("[text0]", actual[0].Data);
            Assert.Equal("[text1]", actual[1].Data);
            Assert.Equal("[text2]", actual[2].Data);

            Assert.Equal(jobItem, actual[0].Job);
            Assert.Equal(jobItem, actual[1].Job);
            Assert.Equal(jobItem, actual[2].Job);

            Assert.Equal(DataBlockType.Link, actual[0].BlockType);
            Assert.Equal(DataBlockType.Link, actual[1].BlockType);
            Assert.Equal(DataBlockType.Link, actual[2].BlockType);
        }

        [Fact(DisplayName = "Should store data")]
        public void Should_store_date()
        {
            #region arrange data

            var mockData = new Mock<IDataBlocksRepository>();
            mockData.Setup(m => m.StoreData(It.IsAny<Process>(), It.IsAny<DataBlockType>(), It.IsAny<string>())).Returns(Guid.Empty);

            const string testUrl = "url";
            const string testData = "text";
            var jobItem = new Process
            {
                Domain = new DomainName
                {
                    Url = testUrl
                }
            };

            #endregion

            var webClientFactory = Mock.Of<IWebClientFactory>();
            var crawlerSettingsRepository = Mock.Of<ICrawlerSettingsRepository>();
            var dataRepository = Mock.Of<IDataBlocksRepository>();
            var pipelineRoutines = new PipelineRoutines(webClientFactory, crawlerSettingsRepository, dataRepository);
            pipelineRoutines.StoreData(new ParsedContentData(jobItem, DataBlockType.Link, testData));

            mockData.Verify(m => m.StoreData(
                It.Is<Process>(job => job == jobItem),
                It.Is<DataBlockType>(dataType => dataType == DataBlockType.Link),
                It.Is<string>(s => s == testData))
                , Times.Once);
        }
    }
}