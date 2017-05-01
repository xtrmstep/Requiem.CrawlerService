using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using AutoMapper;
using CrawlerService.Common.DateTime;
using CrawlerService.Data.Fixtures;
using CrawlerService.Data.Models;
using Moq;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class ProcessesRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public ProcessesRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "Start should create new process domain without running process")]
        public void Start_should_create_new_process_domain_without_running_process()
        {
            using (var ctx = _db.CreateContext())
            {
                // arrange
                var domain = new DomainName
                {
                    Name = "www.test.com"
                };
                ctx.DomainNames.Add(domain);
                ctx.SaveChanges();

                // act
                var actual = new ProcessesRepository(Mock.Of<IMapper>()).Start(domain);

                // assert
                Assert.NotNull(actual);
                Assert.Equal(domain.Name, actual.Domain.Name);
                Assert.Equal(Types.Statuses.IN_PROGRESS, actual.Status);
            }
        }

        [Fact(DisplayName = "JobRepository should create new job for old url")]
        public void Should_create_new_job_for_url_which_has_inactive_job()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should not create new job if already exists one")]
        public void Should_return_null_if_job_is_already_running_for_url()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should add log message when job is started")]
        public void Should_add_log_message_when_job_is_started()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should add log message when job is completed")]
        public void Should_add_item_to_history_when_job_is_completed()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should mark job finished when it is completed")]
        public void Should_mark_job_finished_when_it_is_completed()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should add log message when job is stopped")]
        public void Should_add_item_to_history_when_job_is_stopped()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should mark job finished when it is stopped")]
        public void Should_mark_job_finished_when_it_is_stopped()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should release URL when job is completed")]
        public void Should_release_URL_when_job_is_completed()
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "JobRepository should release URL when job  is stopped")]
        public void Should_release_URL_when_job_is_stopped()
        {
            throw new NotImplementedException();
        }
    }
}