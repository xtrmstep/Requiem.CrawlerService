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
        public string[] Allow { get; set; }
        public string[] Disallow { get; set; }
        public float? CrawlDelay { get; set; }

        public IList<Process> Processes { get; set; } = new List<Process>();
    }
}