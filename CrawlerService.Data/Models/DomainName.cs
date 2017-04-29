using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrawlerService.Data.Models
{
    public class DomainName
    {
        [Key]
        public string Name { get; set; }

        public DateTime? EvaliableFromDate { get; set; }
        public string RobotsTxtPath { get; set; }
        public string Disallow { get; set; }
        public int? CrawlDelay { get; set; }
    }
}