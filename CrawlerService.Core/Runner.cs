using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Impl;
using CrawlerService.Types;
using CrawlerService.Types.Dataflow;

namespace CrawlerService.Core
{
    public class Runner
    {
        static Runner()
        {
            ImplementationModule.Init();
        }

        public Runner()
        {
            BatchSize =
                MaxThreadsNumber = 3;
        }

        /// <summary>
        /// Number of URLs picked up from the frontier at one go
        /// </summary>
        public int BatchSize
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum number of threads to be used to process the batch of URLs
        /// </summary>
        public int MaxThreadsNumber
        {
            get;
            set;
        }

        public void Run()
        {
            var urlFrontier = ServiceLocator.Resolve<IUrlFrontierRepository>();
            var jobs = ServiceLocator.Resolve<IJobRepository>();
            var pipeline = ServiceLocator.Resolve<IPipeline>();

            #region dataflow blocks

            var downloadUrl = new TransformBlock<JobItem, DownloadedContentData>(job => pipeline.DownloadContent(job), new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = MaxThreadsNumber
            });
            var getRules = new TransformManyBlock<DownloadedContentData, ParsingRulesData>(data => pipeline.GetParsingRules(data));
            var parseContent = new TransformManyBlock<ParsingRulesData, ParsedContentData>(data => pipeline.ParseContent(data));
            var storeData = new TransformBlock<ParsedContentData, JobItem>(data => pipeline.StoreData(data));
            var sinkBlock = new ActionBlock<JobItem>(job => { });

            #endregion

            #region pipeline configuration

            downloadUrl.LinkTo(getRules, new DataflowLinkOptions { PropagateCompletion = true });
            getRules.LinkTo(parseContent, new DataflowLinkOptions { PropagateCompletion = true });
            parseContent.LinkTo(storeData, new DataflowLinkOptions { PropagateCompletion = true });
            storeData.LinkTo(sinkBlock, new DataflowLinkOptions { PropagateCompletion = true });

            #endregion

            // obtain a bunch of URLs and start processing in parallel (as per configuration)
            var startedJobs = new List<JobItem>();
            var urlItems = urlFrontier.GetAvailableUrls(BatchSize, DateTime.UtcNow);
            foreach (var urlItem in urlItems)
            {
                var jobItem = jobs.Start(urlItem);
                startedJobs.Add(jobItem);
                downloadUrl.Post(jobItem);
            }
            downloadUrl.Complete();

            // wait until the pipeline is completed and mark all jobs finished
            sinkBlock.Completion.Wait();
            foreach (var job in startedJobs)
            {
                jobs.Complete(job);
            }
        }
    }
}
