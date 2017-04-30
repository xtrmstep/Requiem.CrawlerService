using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using CrawlerService.Common.DateTime;
using CrawlerService.Data;
using CrawlerService.Data.Models;
using CrawlerService.Types.Dataflow;

namespace CrawlerService.Core
{
    public class Runner
    {
        private readonly IProcessesRepository _jobs;
        private readonly IPipeline _pipeline;
        private readonly IDomainNamesRepository _urlFrontier;

        public Runner(IDomainNamesRepository urlFrontierRepository, IProcessesRepository jobRepository, IPipeline pipeline)
        {
            _urlFrontier = urlFrontierRepository;
            _pipeline = pipeline;
            _jobs = jobRepository;
            BatchSize =
                MaxThreadsNumber = 3;
        }

        /// <summary>
        ///     Number of URLs picked up from the frontier at one go
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        ///     Maximum number of threads to be used to process the batch of URLs
        /// </summary>
        public int MaxThreadsNumber { get; set; }

        public void Run()
        {
            #region dataflow blocks

            var downloadUrl = new TransformBlock<Process, DownloadedContentData>(job => _pipeline.DownloadContent(job), new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = MaxThreadsNumber
            });
            var getRules = new TransformManyBlock<DownloadedContentData, ParsingRulesData>(data => _pipeline.GetParsingRules(data));
            var parseContent = new TransformManyBlock<ParsingRulesData, ParsedContentData>(data => _pipeline.ParseContent(data));
            var storeData = new TransformBlock<ParsedContentData, Process>(data => _pipeline.StoreData(data));
            var sinkBlock = new ActionBlock<Process>(job => { });

            #endregion

            #region pipeline configuration

            downloadUrl.LinkTo(getRules, new DataflowLinkOptions {PropagateCompletion = true});
            getRules.LinkTo(parseContent, new DataflowLinkOptions {PropagateCompletion = true});
            parseContent.LinkTo(storeData, new DataflowLinkOptions {PropagateCompletion = true});
            storeData.LinkTo(sinkBlock, new DataflowLinkOptions {PropagateCompletion = true});

            #endregion

            // obtain a bunch of URLs and start processing in parallel (as per configuration)
            var startedJobs = new List<Process>();
            var urlItem = _urlFrontier.GetNextDomain(CrawlerDateTime.Now);
            var jobItem = _jobs.Start(urlItem);
            startedJobs.Add(jobItem);
            downloadUrl.Post(jobItem);
            downloadUrl.Complete();

            // wait until the pipeline is completed and mark all jobs finished
            sinkBlock.Completion.Wait();
            foreach (var job in startedJobs)
            {
                _jobs.Complete(job);
            }
        }
    }
}