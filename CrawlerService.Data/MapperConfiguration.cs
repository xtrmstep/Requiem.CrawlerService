using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CrawlerService.Data.Models;

namespace CrawlerService.Data
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<DomainName, DomainName>();
            CreateMap<Process, Process>();
            CreateMap<DataBlock, DataBlock>();
            CreateMap<ExtractRule, ExtractRule>();
        }
    }
}
