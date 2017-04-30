using System;
using System.Linq;
using AutoMapper;
using CrawlerService.Common.DateTime;
using CrawlerService.Data.Fixtures;
using CrawlerService.Data.Models;
using Moq;
using Xunit;

namespace CrawlerService.Data.Impl
{
    [Collection("DbIntegratedTests")]
    public class DomainNamesRepositoryTests
    {
        private readonly DatabaseFixture _db;

        public DomainNamesRepositoryTests(DatabaseFixture db)
        {
            _db = db;
        }

        [Fact(DisplayName = "GetNextDomain should return domain with null available date first")]
        public void GetNextDomain_should_return_domain_with_null_available_date_first()
        {
            using (var ctx = _db.CreateContext())
            {
                //arrange
                var evaliableFromDate = new DateTime(2016, 1, 19, 21, 0, 0, DateTimeKind.Utc);
                var asOfDate = new DateTime(2016, 1, 20, 21, 0, 0, DateTimeKind.Utc);
                var domain = new DomainName
                {
                    Name = "www.test-no-date.com"
                };
                ctx.DomainNames.Add(domain);
                domain = new DomainName
                {
                    Name = "www.test-with-date.com",
                    EvaliableFromDate = evaliableFromDate
                };
                ctx.DomainNames.Add(domain);
                ctx.SaveChanges();

                var mapper = Mock.Of<IMapper>();

                //act
                var actual = new DomainNamesRepository(mapper).GetNextDomain(asOfDate);

                //assert
                Assert.NotNull(actual);
                Assert.Equal("www.test-no-date.com", actual.Name);
                Assert.False(actual.EvaliableFromDate.HasValue);
            }
        }

        [Fact(DisplayName = "GetNextDomain should return domain with first available date first")]
        public void GetNextDomain_should_return_domain_with_first_available_date_first()
        {
            using (var ctx = _db.CreateContext())
            {
                //arrange
                var evaliableFromDate1 = new DateTime(2016, 1, 18, 21, 0, 0, DateTimeKind.Utc);
                var evaliableFromDate2 = new DateTime(2016, 1, 19, 21, 0, 0, DateTimeKind.Utc);
                var asOfDate = new DateTime(2016, 1, 20, 21, 0, 0, DateTimeKind.Utc);
                var domain = new DomainName
                {
                    Name = "www.test1.com",
                    EvaliableFromDate = evaliableFromDate1
                };
                ctx.DomainNames.Add(domain);
                domain = new DomainName
                {
                    Name = "www.test2.com",
                    EvaliableFromDate = evaliableFromDate2
                };
                ctx.DomainNames.Add(domain);
                ctx.SaveChanges();

                //act
                var actual = new DomainNamesRepository(Mock.Of<IMapper>()).GetNextDomain(asOfDate);

                //assert
                Assert.NotNull(actual);
                Assert.Equal("www.test1.com", actual.Name);
                Assert.Equal(evaliableFromDate1, actual.EvaliableFromDate.Value);
            }
        }

        [Fact(DisplayName = "GetNextDomain should return domain with first available date first")]
        public void GetNextDomain_should_return_domain_with_available_date_and_no_running_process()
        {
            using (var ctx = _db.CreateContext())
            {
                //arrange
                var evaliableFromDate1 = new DateTime(2016, 1, 18, 21, 0, 0, DateTimeKind.Utc);
                var evaliableFromDate2 = new DateTime(2016, 1, 19, 21, 0, 0, DateTimeKind.Utc);
                var asOfDate = new DateTime(2016, 1, 20, 21, 0, 0, DateTimeKind.Utc);
                var domain1 = new DomainName
                {
                    Name = "www.test1.com",
                    EvaliableFromDate = evaliableFromDate1
                };
                ctx.DomainNames.Add(domain1);
                var domain2 = new DomainName
                {
                    Name = "www.test2.com",
                    EvaliableFromDate = evaliableFromDate2
                };
                ctx.DomainNames.Add(domain2);
                ctx.SaveChanges();

                // running process for the 1st domain
                var process = new Process
                {
                    DateStart = CrawlerDateTime.Now.AddSeconds(-100),
                    Domain = domain1,
                    Status = Types.Statuses.IN_PROGRESS
                };
                ctx.Processes.Add(process);
                ctx.SaveChanges();

                //act
                var actual = new DomainNamesRepository(Mock.Of<IMapper>()).GetNextDomain(asOfDate);

                //assert
                Assert.NotNull(actual);
                Assert.Equal("www.test2.com", actual.Name);
                Assert.Equal(evaliableFromDate2, actual.EvaliableFromDate.Value);
            }
        }
    }
}