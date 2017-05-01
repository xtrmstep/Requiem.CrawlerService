using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        [Fact(DisplayName = "Start should fail to create new process for domain if there is other running one")]
        public void Start_should_fail_to_create_new_process_for_domain_if_there_is_other_running_one()
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

                var process = new Process
                {
                    Domain = domain,
                    Status = Types.Statuses.IN_PROGRESS,
                    DateStart = new DateTime(2016, 1, 30, 21, 0, 0, DateTimeKind.Utc) // some date in the past
                };
                ctx.Processes.Add(process);
                ctx.SaveChanges();

                // act & assert
                var exception = Assert.Throws<Exception>(() => new ProcessesRepository(Mock.Of<IMapper>()).Start(domain));
                Assert.Equal("Already in progress", exception.Message);
            }
        }

        [Fact(DisplayName = "Start should create only one process in concurrent environment")]
        public void Start_should_create_only_one_process_in_concurrent_environment()
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
                Process process1 = null;
                Process process2 = null;
                var mre = new ManualResetEvent(false);

                var t1 = Task.Run(() =>
                {
                    mre.WaitOne();

                    using (var internalCtx = _db.CreateContext())
                    {
                        var existingDomain = internalCtx.DomainNames.Single(d => d.Name == "www.test.com");
                        process1 = new ProcessesRepository(Mock.Of<IMapper>()).Start(existingDomain);
                    }
                });
                var t2 = Task.Run(() =>
                {
                    mre.WaitOne();

                    using (var internalCtx = _db.CreateContext())
                    {
                        var existingDomain = internalCtx.DomainNames.Single(d => d.Name == "www.test.com");
                        process2 = new ProcessesRepository(Mock.Of<IMapper>()).Start(existingDomain);
                    }
                });
                mre.Set();
                Task.WaitAll(t1, t2);

                Assert.NotNull(process1);
                Assert.Null(process2);
            }
        }
    }
}